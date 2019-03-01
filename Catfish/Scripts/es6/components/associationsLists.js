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
            // list of selected entity ids. Can span multiple pages
            selected: [],
            page: 1,            
            totalPages: 1,
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
        this.allActions = []
        this.updateSelected = this.updateSelected.bind(this)
        this.updatePage = this.updatePage.bind(this)
        //this.updatePageAll = this.updatePageAll.bind(this)
        //this.updatePageChildren = this.updatePageChildren.bind(this)
        //this.updatePageParents = this.updatePageParents.bind(this)
        this.addChildren = this.addChildren.bind(this)
        this.addParents = this.addParents.bind(this)
        this.removeParents = this.removeParents.bind(this)
        this.removeChildren = this.removeChildren.bind(this)
        this.clearSelected = this.clearSelected.bind(this)
        this.clearAllSelected = this.clearAllSelected.bind(this)
        this.log = this.log.bind(this)
        this.initActions();

    }

    log(x) {
        console.log(x)
    }

    updateSelected(location, payload) {
        const newState = update(this.state,
        {
            [location]: { selected: { $set: payload } }
        })        
        
        this.setState(newState)        
    }

    updatePage(location, payload) {
        //const url = '/apix/Aggregations'
        const { page } = payload
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

        axios.get(url, {
            params: {
                page,
                id
            }
        })
            .then(response => {
                const data = response.data
                const newState = update(this.state, {
                    [location]: {
                        page: { $set: page },
                        data: { $set: data.data }
                    }
                })
                this.setState(newState)
            })
    }

    updatePageAll(location, payload) {        
        const url = '/apix/Aggregations'
        const { page } = payload.page

        axios.get(url, {
            params: {
                page: page
            }
        })
        .then(response => {
            const data = response.data
            const newState = update(this.state, {
                [location]: {
                    page: { $set: page },
                    data: {$set: data.data}
                }
            })                
            this.setState(newState)
        })
        
    }

    updatePageChildren(location, payload) {
        const url = '/apix/Aggregations/getChildren'
        const { page } = payload

        axios.get(url, {
            params: {
                page: page,
                id: external.modelId
            }
        })
        .then(response => {
            const data = response.data
            const newState = update(this.state, {
                [location]: {
                    page: { $set: page },
                    data: { $set: data.data }
                }
            })
            this.setState(newState)
        })
    }

    updatePageParents(location, payload) {
        const url = '/apix/Aggregations/getParents'
        const { page } = payload

        axios.get(url, {
            params: {
                page: page,
                id: external.modelId
            }
        })
            .then(response => {
                const data = response.data
                const newState = update(this.state, {
                    [location]: {
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

    searchAll(entry) {
        
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

        const allStyle = {
            width: '50%',
            float: 'left'
        }

        

        return <div style={div100Style}>
                 

            <div style={allStyle}>
                <div>{all.title}</div>
                <ActionableInputField
                    handleChange={this.log}
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
                />
                <Pagination
                    location="all"
                    page={all.page}
                    totalPages={all.totalPages}
                    update={this.updatePage}
                />


            </div>

            <div>

                <div>{children.title}</div>
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
                />

                <Pagination
                    location="children"
                    page={children.page}
                    totalPages={children.totalPages}
                    update={this.updatePage}
                />
            </div>
            <div>
                <div>{parents.title}</div>

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
                />

                <Pagination
                    location="parents"
                    page={parents.page}
                    totalPages={parents.totalPages}
                    update={this.updatePage}
                />
            </div>
        </div>

        //return <div>
        //    <div>{allData.title}</div>
        //    <ActionButtons
        //        actions={this.allActions}
        //        data={this.state[allListKey].selected}
        //        />
        //    <ActionableTable
        //            listKey={allListKey}
        //            data={allData}
        //            toggle={this.toggle}
        //            isChecked={this.isChecked}
        //            toggleAll={this.toggleAll}
        //            toggleAll={this.toggleAll}
        //            areAllChecked={this.areAllChecked}
        //    />

        //    <div>{parentsData.title}</div>
        //    <ActionButtons
        //        actions={this.parentsActions}
        //        data={this.state[parentsListKey].selected}
        //    />
        //    <ActionableTable
        //        listKey={parentsListKey}
        //        data={parentsData}
        //        toggle={this.toggle}
        //        isChecked={this.isChecked}
        //        toggleAll={this.toggleAll}
        //        areAllChecked={this.areAllChecked}
        //    />

        //    <Pagination
        //        currentPage={allData.currentPage}
        //        totalPages={allData.totalPages}
        //        delta={2}
        //        goToPage={this.goToPage}
        //        listKey={allListKey}
        //        delta={2}
        //        />
        //    </div>
    }

}
