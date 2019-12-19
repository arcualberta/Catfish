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
import ActionableDropDownList from './actionableDropDownList'


export default class CollectionsLists extends React.Component {

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
                 key: "id",
                 title: "Id"
             },
           
            {
                id: 2,
                key: "entityType",
                title: "Entity type"
            },
            {
                id: 3,
                key: "links",
                title: "Actions"
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
            type:modelType, //CFItem OR CFCollection
            selectedEntityType:""
           
        }
        //link for each item/collection that to be set on the actinable table
        this.itemLink =("/manager/" + modelType.substring(2) + "s/associations/") 
        this.state = {
            all: { ...startingState }
                 }
                        //this.allActions = []
             this.updateSelected = this.updateSelected.bind(this)
            this.updatePage = this.updatePage.bind(this)
            
            this.handleSearch = this.handleSearch.bind(this)
            this.handleSearchAll = this.handleSearchAll.bind(this)
            this.addAssociation = this.addAssociation.bind(this)
            this.updateCollection = this.updateCollection.bind(this)
            this.downloadData = this.downloadData.bind(this)
            this.deleteCollection = this.deleteCollection.bind(this)
            this.viewAccessGroup = this.viewAccessGroup.bind(this)

            this.handleSearchEntityType = this.handleSearchEntityType.bind(this)
            this.searchByEntityType = this.searchByEntityType.bind(this)
            this.searchTimeDelay = 200

            this.searchTimers = {
                'all': null
              }
           this.initActions()


}

handleSearchAll(x) {
    this.handleSearch({ ...x }, 'all')
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
handleSearchEntityType(x)
{
    this.searchByEntityType({ ...x }, 'all')
}
searchByEntityType(x, location)
{  
    const entityType  = x.target.value
  
   if (this.searchTimers[location] !== null) {
       clearTimeout(this.searchTimers[location])
       this.searchTimers[location] = null;
   }

   this.searchTimers[location] = setTimeout(() => {
       const page = 1
       const query = "*:*"
       this.updatePage(location, { page, query, entityType  })
       this.searchTimers[location] = null;
   }, this.searchTimeDelay)
}

updateCollection(selectedId) { 
   
    window.location = "/manager/"+ controller + "/edit/"+ selectedId;
    
}

addAssociation(selectedId){   
         
    window.location= "/manager/"+ controller +"/associations/"+ selectedId;   
}
downloadData(selectedId)
{
   
    window.location = "/manager/"+ controller +"/DownloadFormData/"+ selectedId;
  
  
}

deleteCollection(selectedId)
{
    if(confirm("This will delete the entity " + selectedId + ". Do you wish to continue?")){
        axios.post('/manager/' + controller + '/delete/', {
            id: selectedId
        }).then(function (response) {
                window.location.reload();
            })
            .catch(error => console.log(error));
    }
}

viewAccessGroup(selectedId){
    window.location = "/manager/"+ controller +"/accessGroup/" + selectedId
}


initActions() {
    this.allActions = [
       /* {
            title: "Edit",
            action: this.updateCollection,
            id: "edit-collection-action"
        },
        {
            title: "Association",
            action: this.addAssociation,
            id: "add-asspciation-action"
        }*/
    ]
  
}

getPageParameters(location) {
    let url
    let id

    switch (location) {
        case 'all':
        default:
            url = '/apix/Aggregations'
            break

    }
    return { url, id }
}

updatePage(location, payload) {
    //const url = '/apix/Aggregations'
    let { page, query, entityType } = payload
    const { url, id } = this.getPageParameters(location)

    if (page == null) {
        page = this.state[location].page;
    }

    if (query == null) {
        query = this.state[location].query;
    }

    if (entityType == null) {
        entityType = this.state[location].selectedEntityType;
    }

    axios.get(url, {
        params: {
            id,
            Query: query,
            Page: page,
            ItemsPerPage: this.state[location].itemsPerPage,
            Type: modelType,
            EntityType: entityType
            
        }
    })
        .then(response => {
            const data = response.data
            const links = [
              {
                  title: "<span class='glyphicon glyphicon-edit object-edit' />",
                  action: this.updateCollection        
              },
               {
                   title: "<span class='glyphicon glyphicon-link object-associations' />",
                   action: this.addAssociation          
               },
                {
                    title: "<span class='glyphicon glyphicon-remove object-delete' />",
                    action: this.deleteCollection
                   
                },
                {
                    title: "<span class='glyphicon glyphicon-eye-close object-accessgroup' />",
                    action: this.viewAccessGroup
                   
                }
            ]
            const dataData = response.data.data.map(x => {x.links=(
                <ActionButtons actions={links} payload={x.id} />
               )
              
                return x
            })
            const newState = update(this.state, {
                [location]: {
                    query: { $set: query },
                    page: { $set: page },
                    data: { $set: dataData},
                    totalPages: { $set: data.totalPages },
                    selectedEntityType:{$set: entityType}
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
            ItemsPerPage: 10,
            Type:modelType
        }
    })
       .then(response => {

           // this should be replaced with the incoming data from api call
           const data = response.data
           const links = [
               {
                   title: "<span class='glyphicon glyphicon-edit object-edit' />",
                   action: this.updateCollection      
               },
                {
                    title: "<span class='glyphicon glyphicon-link object-associations' />",
                    action: this.addAssociation        
                },
                 {
                     title: "<span class='glyphicon glyphicon-remove object-delete' />",
                     action: this.deleteCollection      
                 },
                 {
                     title: "<span class='glyphicon glyphicon-eye-close object-accessgroup' />",
                     action: this.viewAccessGroup  
                 }
           ]
           const dataData = response.data.data.map(x => {x.links=(
                 <ActionButtons actions={links} payload={x.id} />
               )
                     
               return x
           })
        
            const newState = update(this.state,
                {
                    all: {                            
                        title: { $set: "All" },
                        selected: { $set: [] },
                        page: { $set: data.page },
                        itemsPerPage: { $set: data.itemsPerPage },
                        totalPages: { $set: data.totalPages },
                        headers: { $set: this.basicHeaders },
                        data: { $set: dataData},
                        selectedEntityType: {$set: ""}
                       
                       
                    }
                });
            this.setState(newState)

        })
}

render() {

    const all = this.state.all
    

    const div100Style = {
        width: '100%'
    }

    return(
              <div className="bs container">
                  <div className="row">
                      <div className="col-md-12">
                          <form className="form-horizontal">
                              <div className="form-group">
                                  <label className="col-sm-2 control-label">{all.title}</label>
                                  <div className="col-sm-4">
                                      <ActionableInputField
                                            handleChange={this.handleSearchAll}
                                            actions={this.allActions}
                                            placeholder="Search"
                                            id="all-search"
                                    />
                                 </div>
                              </div>
                           </form>
                           <form className="form-horizontal">
                              <div className="form-group">
                                  <label className="col-sm-2 control-label">EntityType :</label>
                                            <div className="col-sm-4">
                                                <ActionableDropDownList
                                                       handleChange={this.handleSearchEntityType}
                                                       actions={this.allActions}
                                                        data={EntityTypes}
                                                        selectedType={this.selectedEntityType}
                                                        id="entitytype-search"
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
                                    itemLink ={this.itemLink}

                                />
                                
                           <Pagination
                                    location="all"
                                    page={all.page}
                                    totalPages={all.totalPages}
                                    update={this.updatePage}
                                    id="all-pagination"
                                />
                      </div>
                  </div>
              </div>
            
            );

    }
}
