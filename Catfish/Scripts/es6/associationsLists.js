import React from "react"
import ReactDOM from "react-dom"
import PropTypes from 'prop-types'
import axios from 'axios'
import ActionableTable from './actionableTable'
import update from 'immutability-helper'
import ActionButtons from './actionButtons'
import { objectify } from './helpers'
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
            totalItems: 1,
            itemsPerPage: 10,
            // N headers to be used as table headers
            // {
            //   id: int,
            //   key: hash key,
            //   title: Title to show as column header
            //}
            headers: [],
            // list of entities to show on a page
            // {
            //   id: int,
            //   name: Name of entity,
            //   entityType: Entity type defined on catfish,
            //   type: Type of entity, (collection, item)
            //  }
            entities: [],
            // list of actions to perform on selected entities
            // {
            //   title: title on action,
            //   action: function to execute on selected entities
            //}
            actions: []
        }

        this.state = {
            all: objectify(startingState),
            parents: objectify(startingState),
            children: objectify(startingState),
            relations: objectify(startingState)
        }

        this.toggle = this.toggle.bind(this);
        this.toggleAll = this.toggleAll.bind(this);
        this.isChecked = this.isChecked.bind(this);
        this.areAllChecked = this.areAllChecked.bind(this);

        this.initActions();
    }

    isChecked(listkey, id) {
        const { selected } = this.state[listkey];
        return selected.includes(id);
    }

    toggle(listKey, id) {
        let selected = this.state[listKey].selected.map( x => x );
        const index = selected.indexOf(id);

        if (index === -1) {
            selected.push(id)
        } else {
            selected.splice(index, 1)
        }

        const currentState = update(this.state,
            {
                [listKey]: { selected: { $set: selected } }
            })
        this.setState(currentState)

    }

    toggleAll(listKey, checked) {

        let selected = [] 
        
        if (checked) {
                                    
            let missingEntities = this.state[listKey].entities
                .filter(x => {
                    return !this.state[listKey].selected.includes(x.id)
                })
                .map(x => x.id)
          
            selected = [...this.state[listKey].selected, ...missingEntities]
        } else {           
            selected = this.state[listKey].selected.filter(x => {
                this.state[listKey].entities.includes(x)
            })
        }
       
        const currentState = update(this.state, {
            [listKey]: { selected: { $set: selected } }
        })

        this.setState(currentState)
    }

    areAllChecked(listKey) {

        const { entities, selected } = this.state[listKey]
        
        let result = true
        entities.forEach((entity) => {
            if (!selected.includes(entity.id)) {
                result = false;
                return;
            }
        })

        return result;
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
                        totalItems: 5,
                        itemsPerPage: 10,
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
                        entities: [
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
                        totalItems: 1,
                        itemsPerPage: 10,
                        headers: [{
                            id: 0,
                            key: "name",
                            title: "Name"
                        }                            
                        ],
                        entities: [
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
                action: (selected) => { console.log("Add " + selected) }
            },
            {
                title: "Remove",
                action: (selected) => { console.log("Remove " + selected) }
            }
        ]

        this.parentsActions                  = [
            {
                title: "Compare",
                action: (selected) => { console.log("Compare " + selected) }
            }
        ]

        this.goToPage = (page) => {
            console.log(page)
        }

    }

    render() {

        const allListKey = "all";
        const allData = this.state[allListKey];        
        const parentsListKey = "parents";
        const parentsData = this.state[parentsListKey];
        
        return <div>
            <div>{allData.title}</div>
            <ActionButtons
                actions={this.allActions}
                data={this.state[allListKey].selected}
                />
            <ActionableTable
                    listKey={allListKey}
                    data={allData}
                    toggle={this.toggle}
                    isChecked={this.isChecked}
                    toggleAll={this.toggleAll}
                    toggleAll={this.toggleAll}
                    areAllChecked={this.areAllChecked}
            />

            <div>{parentsData.title}</div>
            <ActionButtons
                actions={this.parentsActions}
                data={this.state[parentsListKey].selected}
            />
            <ActionableTable
                listKey={parentsListKey}
                data={parentsData}
                toggle={this.toggle}
                isChecked={this.isChecked}
                toggleAll={this.toggleAll}
                areAllChecked={this.areAllChecked}
            />

            <Pagination
                currentPage={allData.currentPage}
                totalPages={10}
                goToPage={this.goToPage}
                delta={2}
                />
            </div>
    }

}
