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

        this.basicHeaders = [
            {
                id: 0,
                key: "name",
                title: "Name"
            },
            {
                id: 1,
                key: "entityType",
                title: "Entity type"
            },
            {
                id: 2,
                key: "label",
                title: "Type"
            }
        ]

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
            headers: this.basicHeaders,
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
            actions: [],
            disabled: false
          
        }

        this.state = {
            all: { ...startingState },
            parents: { ...startingState },
            children: { ...startingState },
            related: { ...startingState }
        }
        //this.allActions = []
        this.updateSelected = this.updateSelected.bind(this)
        this.updatePage = this.updatePage.bind(this)
        this.addChildren = this.addChildren.bind(this)
        this.addParents = this.addParents.bind(this)
        this.addRelated = this.addRelated.bind(this)
        this.removeParents = this.removeParents.bind(this)
        this.removeChildren = this.removeChildren.bind(this)
        this.removeRelated = this.removeRelated.bind(this)
        this.clearSelected = this.clearSelected.bind(this)
        this.clearAllSelected = this.clearAllSelected.bind(this)
        this.handleSearch = this.handleSearch.bind(this)
        this.handleSearchAll = this.handleSearchAll.bind(this)
        this.handleSearchParents = this.handleSearchParents.bind(this)
        this.handleSearchChildren = this.handleSearchChildren.bind(this)
        this.handleSearchRelated = this.handleSearchRelated.bind(this)
        this.initActions()


        this.searchTimeDelay = 200

        this.searchTimers = {
            'all': null,
            'children': null,
            'parents': null,
            'related': null
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

    handleSearchRelated(x) {
        this.handleSearch({ ...x }, 'related')
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
                [location]: { selected: { $set: payload }}
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
            case 'related':
                url = '/apix/Aggregations/getRelated'
                id = external.modelId
                break
            default:
                url = '/apix/Aggregations'
                break

        }
        return { url, id }
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
                page,
                itemsPerPage: this.state[location].itemsPerPage
            }
        })
            .then(response => {
                const data = response.data
                const newState = update(this.state, {
                    [location]: {
                        query: { $set: query },
                        page: { $set: page },
                        data: { $set: data.data },
                        totalPages: { $set: data.totalPages }
                    }
                })
                this.setState(newState)
                console.log(newState)
            })
    }

    isEquivalentData(a, b) {
        if (a.id === b.id) {
            return true
        }

        return false
    }

    componentDidMount() {
        axios.get('/apix/Aggregations', {
            params: {
                ItemsPerPage: 10
            }
        })
            .then(response => {

                // this should be replaced with the incoming data from api call
                const data = response.data
                const newState = update(this.state,
                    {
                        all: {                            
                            title: { $set: "All" },
                            selected: { $set: [] },
                            page: { $set: data.page },
                            itemsPerPage: { $set: data.itemsPerPage },
                            totalPages: { $set: data.totalPages },
                            headers: { $set: this.basicHeaders },
                            data: { $set: data.data }
                        }
                    });
                this.setState(newState)

                //this.setState(resp)
            })
        axios.get('/apix/Aggregations/getChildren', {
            params: {
                id: external.modelId,
                ItemsPerPage: 5
            }
        })
            .then(response => {
                const data = response.data
                const newState = update(this.state,
                    {
                        children: {
                            title: { $set: "Children" },
                            selected: { $set: [] },
                            page: { $set: data.page },
                            itemsPerPage: { $set: data.itemsPerPage },
                            totalPages: { $set: data.totalPages },
                            headers: { $set: this.basicHeaders },
                            data: { $set: data.data }
                        }
                    });
                this.setState(newState)
            })
        axios.get('/apix/Aggregations/getParents', {
            params: {
                id: external.modelId,
                ItemsPerPage: 5
            }
        })
            .then(response => {
                const data = response.data
                const newState = update(this.state,
                    {
                        parents: {
                            title: { $set: "Parents" },
                            selected: { $set: [] },
                            page: { $set: data.page },
                            itemsPerPage: { $set: data.itemsPerPage },
                            totalPages: { $set: data.totalPages },
                            headers: { $set: this.basicHeaders },
                            data: { $set: data.data }
                        }
                    });
                this.setState(newState)

            })
            .catch(error => console.log(error))

        axios.get('/apix/Aggregations/getRelated', {
            params: {
                id: external.modelId,
                ItemsPerPage: 5
            }
        })
            .then(response => {
                const data = response.data
                const newState = update(this.state,
                    {
                        related: {
                            title: { $set: "Related" },
                            selected: { $set: [] },
                            page: { $set: data.page },
                            itemsPerPage: { $set: data.itemsPerPage },
                            totalPages: { $set: data.totalPages },
                            headers: { $set: this.basicHeaders },
                            data: { $set: data.data }
                        }
                    });
                this.setState(newState)

            })
            .catch(error => console.log(error))
    }

    addChildren(selected) {
        const self = this

        axios.post('/apix/Aggregations/AddChildren', {
            id: external.modelId,
            objectIds: selected.map(x => x.id)
        })
            .then(response => {
                self.updatePage('children', { page: self.state.children.page })
            })
            .catch(error => console.log(error));
    }

    addParents(selected) {
        const self = this
        axios.post('/apix/Aggregations/AddParents', {
            id: external.modelId,
            objectIds: selected.map(x => x.id)
        })
            .then(response => {
                self.updatePage('parents', { page: self.state.parents.page })
            })
            .catch(error => console.log(error));
    }

    addRelated(selected) {
        const self = this
        axios.post('/apix/Aggregations/AddRelated', {
            id: external.modelId,
            objectIds: selected.map(x => x.id)
        })
            .then(response => {
                self.updatePage('related', { page: self.state.related.page })
            })
            .catch(error => console.log(error));
    }

    removeChildren(selected) {
        const self = this
        const location = 'children'
        axios.post('/apix/Aggregations/RemoveChildren', {
            id: external.modelId,
            objectIds: selected.map(x => x.id)
        })
            .then(response => {
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
            .then(response => {
                self.updatePage(location, { page: self.state.parents.page })
                self.clearSelected(location);
            })
    }

    removeRelated(selected) {
        const self = this
        const location = 'related'
        axios.post('/apix/Aggregations/RemoveRelated', {
            id: external.modelId,
            objectIds: selected.map(x => x.id)
        })
            .then(response => {
                self.updatePage(location, { page: self.state.related.page })
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
        //const disabled = state.disabled;
        this.allActions = [
            {
                title: "Add parents",
                action: this.addParents,
                id: "add-parents-action"
            },
            {
                title: "Add children",
                action: this.addChildren,
                id: "add-children-action"
            },
            {
                title: "Add related",
                action: this.addRelated,
                id: "add-related-action",
                //disabled Add Related button if the selected item is a Collection
                //checking if in the selected[] contains at least 1 Collection item -- array.some(condition/value) will return true if the array contains at least 1 item that match the condition/value
                disabled: ()=>{ return this.state.all.selected.some(x=>(x.label == "Collection"))}
            },
            {
                title: "Clear selection",
                action: this.clearAllSelected,
                id: "clear-selection-action"
            }
        ]

        this.parentsActions = [
            {
                title: "Remove",
                action: this.removeParents,
                id: "remove-parents-action"
            }
        ]

        this.childrenActions = [
            {
                title: "Remove",
                action: this.removeChildren,
                id: "remove-children-action"
            }
        ]

        this.relatedActions = [
            {
                title: "Remove",
                action: this.removeRelated,
                id: "remove-related-action"
            }
        ]

    }

    render() {

        const all = this.state.all
        const children = this.state.children
        const parents = this.state.parents
        const related = this.state.related

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



        return (
            <div className="bs container">
                <div className="row">
                    <div className="col-md-6">
                        <form className="form-horizontal">
                            <div className="form-group">
                                <label className="col-sm-2 control-label">{all.title}</label>
                                <div className="col-sm-10">
                                    <ActionableInputField
                                        handleChange={this.handleSearchAll}
                                        placeholder="Search"
                                        id="all-search"
                                    />
                                </div>
                            </div>
                        </form>

                        <ActionableTable
                            location="all"
                            data={all.data}
                            selected={all.selected}
                            update={this.updateSelected}
                            headers={all.headers}
                            isEquivalent={this.isEquivalentData}
                            maxRows={all.itemsPerPage}
                            actions={this.allActions}
                            id="all-actionable-table"
                        />
                        <Pagination
                            location="all"
                            page={all.page}
                            totalPages={all.totalPages}
                            update={this.updatePage}
                            id="all-pagination"
                        />
                    </div>

                    <div className="col-md-6">
                        <div className="row">

                            <form className="form-horizontal">
                                <div className="form-group">
                                    <label className="col-sm-2 control-label">{children.title}</label>
                                    <div className="col-sm-10">
                                        <ActionableInputField
                                            handleChange={this.handleSearchChildren}
                                            placeholder="Search"
                                            id="children-search"
                                        />
                                    </div>
                                </div>
                            </form>

                            <ActionableTable
                                location="children"
                                data={children.data}
                                selected={children.selected}
                                update={this.updateSelected}
                                headers={children.headers}
                                isEquivalent={this.isEquivalentData}
                                maxRows={children.itemsPerPage}
                                actions={this.childrenActions}
                                id="children-actionable-table"
                            />

                            <Pagination
                                location="children"
                                page={children.page}
                                totalPages={children.totalPages}
                                update={this.updatePage}
                                id="children-pagination"
                            />
                        </div>

                        <div className="row">

                            <form className="form-horizontal">
                                <div className="form-group">
                                    <label className="col-sm-2 control-label">{parents.title}</label>
                                    <div className="col-sm-10">
                                        <ActionableInputField
                                            handleChange={this.handleSearchParents}
                                            placeholder="Search"
                                            id="parents-search"
                                        />
                                    </div>
                                </div>
                            </form>

                            <ActionableTable
                                location="parents"
                                data={parents.data}
                                selected={parents.selected}
                                update={this.updateSelected}
                                headers={parents.headers}
                                isEquivalent={this.isEquivalentData}
                                maxRows={parents.itemsPerPage}
                                actions={this.parentsActions}
                                id="parents-actionable-table"
                            />

                            <Pagination
                                location="parents"
                                page={parents.page}
                                totalPages={parents.totalPages}
                                update={this.updatePage}
                                id="parents-pagination"

                            />
                        </div>


                        <div className="row">

                            <form className="form-horizontal">
                                <div className="form-group">
                                    <label className="col-sm-2 control-label">{related.title}</label>
                                    <div className="col-sm-10">
                                        <ActionableInputField
                                            handleChange={this.handleSearchRelated}
                                            placeholder="Search"
                                            id="related-search"
                                        />
                                    </div>
                                </div>
                            </form>

                            <ActionableTable
                                location="related"
                                data={related.data}
                                selected={related.selected}
                                update={this.updateSelected}
                                headers={related.headers}
                                isEquivalent={this.isEquivalentData}
                                maxRows={related.itemsPerPage}
                                actions={this.relatedActions}
                                id="related-actionable-table"
                            />

                            <Pagination
                                location="parents"
                                page={related.page}
                                totalPages={related.totalPages}
                                update={this.updatePage}
                                id="related-pagination"
                            />
                        </div>

                    </div>
                </div>
            </div>
        )

    }
    
  

}
