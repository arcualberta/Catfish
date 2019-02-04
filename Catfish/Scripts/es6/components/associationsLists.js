import React from "react"
import ReactDOM from "react-dom"
import PropTypes from 'prop-types'
import axios from 'axios'
import ActionableTable from './actionableTable'
import update from 'immutability-helper'
import ActionButtons from './actionButtons'
import { clone } from '../helpers'
import Pagination from './pagination'

export default class AssociationsLists extends React.Component {

    constructor(props) {
        super(props)

        const startingState = {
            // Title to render as description for table
            title: "",
            // list of selected entity ids. Can span multiple pages
            selected: [],
            currentPage: 1,            
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

        this.updateSelected = this.updateSelected.bind(this)
        this.updatePage = this.updatePage.bind(this)
        this.initActions();

    }

    updateSelected(location, payload) {
        const newState = update(this.state,
        {
            [location]: { selected: { $set: payload } }
        })        
        
        this.setState(newState)        
    }

    updatePage(location, payload) {
        console.log(location + " " + payload)
        const newState = update(this.state, {
            [location]: { currentPage: { $set: payload } }
        })

        this.setState(newState)
    }

    isEquivalentData(a, b) {
        if (a.id === b.id) {
            return true
        }

        return false
    }

    componentDidMount() {
        axios.get('/')
            .then(() => {

                // this should be replaced with the incoming data from api call
                const response = {
                    all: {
                        title: "All",
                        selected: [],
                        currentPage: 1,
                        totalPages: 22,
                        itemsDelta: 2,
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
                            },
                            {
                                id: 2,
                                key: "type",
                                title: "Type"
                            }
                        ],
                        data: [
                            {
                                id: 3,
                                name: "Item 1",
                                entityType: "Entity 1",
                                type: "Item"
                            },
                            {
                                id: 4,
                                name: "Collection 1",
                                entityType: "Entity 2",
                                type: "Collection"
                            },
                            {
                                id: 5,
                                name: "Item 2",
                                entityType: "Entity 1",
                                type: "Item"
                            },
                        ]
                    },
                    parents: {
                        title: "Parents",
                        selected: [],
                        currentPage: 1,                                                
                        headers: [{
                            id: 0,
                            key: "name",
                            title: "Name"
                        }                            
                        ],
                        data: [
                            {
                                id: 8,
                                name: "Collection 1"
                            },
                            {
                                id: 9,
                                name: "Collection 2"
                            }
                        ]
                    }
                }                

                this.setState(response)
            })
    }

    initActions() {
        this.allActions = [
            {
                title: "Add",
                action: (selected) => { console.log("Add " + selected.map(x => x.id)) }
            },
            {
                title: "Remove",
                action: (selected) => { console.log("Remove " + selected.map(x => x.id)) }
            }
        ]

        this.parentsActions                  = [
            {
                title: "Compare",
                action: (selected) => { console.log("Compare " + selected.map(x => x.id)) }
            }
        ]

    }

    render() {

        const all = this.state.all
        
        return <div>

            <ActionButtons
                actions={this.allActions}
                payload={all.selected}
            />

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
                currentPage={all.currentPage}
                totalPages={all.totalPages}
                update={this.updatePage}
            />

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
