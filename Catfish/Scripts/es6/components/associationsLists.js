import React from "react"
import ReactDOM from "react-dom"
import PropTypes from 'prop-types'
import axios from 'axios'
import ActionableTable from './actionableTable'
import update from 'immutability-helper'
import ActionButtons from './actionButtons'
import { clone } from '../helpers'
import Pagination from './pagination'
import ConditionalRender from './conditionalRender'
import ActionableInputField from './actionableInputField'


export default class AssociationsLists extends React.Component {

    constructor(props) {
        super(props)

        const startingState = {
            // Title to render as description for table
            title: "",
            // list of selected entity ids. Can span multiple pages filtered by 
            // query
            selected: [],
            page: 1,
            itemsPerPage: 1,
            totalPages: 1,
            query: "*:*",
            // N headers to be used as table headers
            // {
            //   id: int,
            //   key: hash key,
            //   title: Title to show as column header
            //}
            headers: [],
            // list of data to show on a page
            // {
            //   id: int,
            //   name: Name of entity,
            //   entityType: Entity type defined on catfish,
            //   type: Type of entity, (collection, item)
            //  }
            data: [],
            // list of actions to perform on selected data
            // {
            //   title: title on action,
            //   action: function to execute on selected data
            //}
            actions: []
        }

        this.state = {
            all: { ...startingState },
            parents: { ...startingState },
            children: { ...startingState },
            relations: { ...startingState }
        }
        //this.allActions = []
        this.updateSelected = this.updateSelected.bind(this)
        this.updatePage = this.updatePage.bind(this)
        this.addChildren = this.addChildren.bind(this)
        this.addParents = this.addParents.bind(this)
        this.removeParents = this.removeParents.bind(this)
        this.removeChildren = this.removeChildren.bind(this)
        this.clearSelected = this.clearSelected.bind(this)
        this.clearAllSelected = this.clearAllSelected.bind(this)
        this.handleSearch = this.handleSearch.bind(this)
        this.handleSearchAll = this.handleSearchAll.bind(this)
        this.handleSearchParents = this.handleSearchParents.bind(this)
        this.handleSearchChildren = this.handleSearchChildren.bind(this)
        this.initActions()


        this.searchTimeDelay = 200

        this.searchTimers = {
            'all': null,
            'children': null,
            'parents': null
        }

    }

    handleSearchAll(x) {
        this.handleSearch({ ...x }, 'all')
    }

    handleSearchParents(x) {
        this.handleSearch({ ...x }, 'parents')
    }

    handleSearchChildren(x) {
        this.handleSearch({ ...x }, 'children')
    }

    handleSearch(x, location) {

        if (this.searchTimers[location] !== null) {
            clearTimeout(this.searchTimers[location])
            this.searchTimers[location] = null;
        }

        this.searchTimers[location] = setTimeout(() => {
            const page = 1
            const query = x.target.value
            this.updatePage(location, { page, query })   
            this.searchTimers[location] = null;
        }, this.searchTimeDelay)             
    }

    updateSelected(location, payload) {
        const newState = update(this.state,
        {
            [location]: { selected: { $set: payload } }
        })        
        
        this.setState(newState)        
    }

    getPageParameters(location) {
        let url
        let id

        switch (location) {
            case 'all':
                url = '/apix/Aggregations'
                break
            case 'children':
                url = '/apix/Aggregations/getChildren'
                id = external.modelId
                break
            case 'parents':
                url = '/apix/Aggregations/getParents'
                id = external.modelId
                break
            default:
                url = '/apix/Aggregations'
                break

        }
        return {url, id}
    }

    updatePage(location, payload) {
        //const url = '/apix/Aggregations'
        let { page, query } = payload
        const { url, id } = this.getPageParameters(location)        

        if (page == null) {
            page = this.state[location].page
        }

        if (query == null) {
            query = this.state[location].query
        }

        axios.get(url, {
            params: {
                id,
                query,
                page                
            }
        })
            .then(response => {
                const data = response.data
                const newState = update(this.state, {
                    [location]: {
                        query: {$set: query},
                        page: { $set: page },
                        data: { $set: data.data }
                    }
                })
                this.setState(newState)
            })
    }

    isEquivalentData(a, b) {
        if (a.id === b.id) {
            return true
        }

        return false
    }

    componentDidMount() {        
        axios.get('/apix/Aggregations')
            .then( response => {                

                // this should be replaced with the incoming data from api call
                const data = response.data
                const resp = {
                    all: {
                        title: "All",
                        selected: [],
                        page: data.page,
                        itemsPerPage: data.itemsPerPage,
                        totalPages: data.totalPages,
                        headers: [
                            {
                                id: 0,
                                key: "name",
                                title: "Name"
                            },
                            {
                                id: 1,
                                key: "entityType",
                                title: "Entity type"
                            }
                        ],
                        data: data.data
                    }
                }                
                this.setState(resp)
            })
        axios.get('/apix/Aggregations/getChildren', {
            params: {
                id: external.modelId
            }
        })
            .then(response => {                
                const data = response.data
                const resp = {
                    children: {
                        title: "Children",
                        selected: [],
                        page: data.page,
                        itemsPerPage: data.itemsPerPage,
                        totalPages: data.totalPages,
                        headers: [
                            {
                                id: 0,
                                key: "name",
                                title: "Name"
                            },
                            {
                                id: 1,
                                key: "entityType",
                                title: "Entity type"
                            }
                        ],
                        data: data.data
                    }
                }
                this.setState(resp)
            })
        axios.get('/apix/Aggregations/getParents', {
            params: {
                id: external.modelId
            }
        })
            .then(response => {               
                const data = response.data
                const resp = {
                    parents: {
                        title: "Parents",
                        selected: [],
                        page: data.page,
                        itemsPerPage: data.itemsPerPage,
                        totalPages: data.totalPages,
                        headers: [
                            {
                                id: 0,
                                key: "name",
                                title: "Name"
                            },
                            {
                                id: 1,
                                key: "entityType",
                                title: "Entity type"
                            }
                        ],
                        data: data.data
                    }
                }
                this.setState(resp)

            })
            .catch(error => console.log(error))
    }

    addChildren(selected) {              
        const self = this

        axios.post('/apix/Aggregations/AddChildren', {
            id: external.modelId,
            objectIds: selected.map(x => x.id)
        })
            .then( response => {                
                self.updatePage('children', { page: self.state.children.page })
            })
            .catch( error => console.log(error) );
    }

    addParents(selected) {
        const self = this        
        axios.post('/apix/Aggregations/AddParents', {
            id: external.modelId,
            objectIds: selected.map(x => x.id)
        })
            .then( response => {                
                self.updatePage('parents', { page: self.state.parents.page })
            })
            .catch( error => console.log(error) );
    }

    removeChildren(selected) {
        const self = this
        const location = 'children'        
        axios.post('/apix/Aggregations/RemoveChildren', {
            id: external.modelId,
            objectIds: selected.map(x => x.id)
        })
            .then( response => {                
                self.updatePage(location, { page: self.state.children.page })
                self.clearSelected(location)
            })
    }

    removeParents(selected) {
        const self = this
        const location = 'parents'
        axios.post('/apix/Aggregations/RemoveParents', {
            id: external.modelId,
            objectIds: selected.map(x => x.id)
        })
            .then( response => {
                self.updatePage(location, { page: self.state.parents.page })
                self.clearSelected(location);
            })
    }

    clearSelected(location) {
        const newState = update(this.state,
            {
                [location]: { selected: { $set: [] } }
            });        
        this.setState(newState)        
    }

    clearAllSelected() {
        const location = "all"
        this.clearSelected(location)
    }

    initActions() {
        this.allActions = [
            {
                title: "Add parents",
                action: this.addParents
            },
            {
                title: "Add children",
                action: this.addChildren 
            },
            {
                title: "Clear selection",
                action: this.clearAllSelected
            }
        ]

        this.parentsActions = [
            {
                title: "Remove",
                action: this.removeParents
            }
        ]

        this.childrenActions = [
            {
                title: "Remove",
                action: this.removeChildren
            }
        ]

    }

    render() {

        const all = this.state.all
        const children = this.state.children
        const parents = this.state.parents

        const div100Style = {
            width: '100%'
        }

        const leftColumnStyle = {
            width: '50%',
            float: 'left'
        }

        const rightColumnStyle = {
            width: '50%',
            float: 'right'
        }

        

        return <div className="bs container">

            <div className="row">
                <div className="col-md-6">

                    <div>{all.title}</div>
                    <ActionableInputField
                        handleChange={this.handleSearchAll}
                        placeholder="Search"
                    />
                    <ConditionalRender condition={all.selected.length > 0}>                
                        <ActionButtons
                            actions={this.allActions}
                            payload={all.selected}
                        />
                    </ConditionalRender>
                    <ActionableTable
                        location="all"
                        data={all.data}
                        selected={all.selected}
                        update={this.updateSelected}
                        headers={all.headers}
                        isEquivalent={this.isEquivalentData}
                        maxRows={all.itemsPerPage}
                    />
                    <Pagination
                        location="all"
                        page={all.page}
                        totalPages={all.totalPages}
                        update={this.updatePage}
                    />
                </div>

                <div className="col-md-6">
                    <div className="row">
                    <div>{children.title}</div>
                    <ActionableInputField
                        handleChange={this.handleSearchChildren}
                        placeholder="Search"
                    />
                    <ConditionalRender condition={children.selected.length > 0}>                
                        <ActionButtons
                            actions={this.childrenActions}
                            payload={children.selected}
                        />
                    </ConditionalRender>
                    <ActionableTable
                        location="children"
                        data={children.data}
                        selected={children.selected}
                        update={this.updateSelected}
                        headers={children.headers}
                        isEquivalent={this.isEquivalentData}
                        maxRows={children.itemsPerPage}
                    />

                    <Pagination
                        location="children"
                        page={children.page}
                        totalPages={children.totalPages}
                        update={this.updatePage}
                    />
                </div>

                <div className="row">
                    <div>{parents.title}</div>
                    <ActionableInputField
                        handleChange={this.handleSearchParents}
                        placeholder="Search"
                    />
                    <ConditionalRender condition={parents.selected.length > 0}>                
                        <ActionButtons
                            actions={this.parentsActions}
                            payload={parents.selected}
                        />
                    </ConditionalRender>

                    <ActionableTable
                        location="parents"
                        data={parents.data}
                        selected={parents.selected}
                        update={this.updateSelected}
                        headers={parents.headers}
                        isEquivalent={this.isEquivalentData}
                        maxRows={parents.itemsPerPage}
                    />

                    <Pagination
                        location="parents"
                        page={parents.page}
                        totalPages={parents.totalPages}
                        update={this.updatePage}
                    />
                </div>
                    </div>
            </div>
        </div>


    }

}
