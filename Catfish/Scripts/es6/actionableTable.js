import React from "react"
import ReactDOM from "react-dom"
import PropTypes from 'prop-types'


//location = "all"
//data = { all.entities }
//selected = { all.selected }
//update = { this.updateSelected }
//headers = { all.headers }
//isEquivalent = { this.isEquivalentData }

const ActionableTable = ({
    location,
    data,
    selected,
    update,
    headers,
    isEquivalent
}) => {
    const renderParameters = { headers, data, selected, isEquivalent, update, location}
    return (
        <table>
            {renderHead(renderParameters)}
            {renderBody(renderParameters)}            
        </table>
        )
}

const renderHead = ({ headers, data, selected, isEquivalent, update, location }) => 
    <thead>
        <tr>

            <td>
                <input
                    type="checkbox"
                    checked={isPageChecked({ data, selected, isEquivalent })}
                    onChange={(event) => {
                        const checked = event.target.checked
                        togglePage({ data, selected, isEquivalent, update, location, checked })
                    } }
                />
            </td>

            {headers.map(x =>
                <td key={x.id}>{x.title}</td>
            )}
        </tr>
    </thead>


const renderBody = ({data, headers, selected, isEquivalent, update, location }) => 
    <tbody>
        {
            data.map(datum =>
                <tr key={datum.id}>

                    <td>
                        <input
                            type="checkbox"
                            checked={isChecked({ datum, selected, isEquivalent })}
                            onChange={() => {
                                toggle({ datum, selected, isEquivalent, update, location })
                            }}
                        />
                    </td>


                    {headers.map(header =>
                        <td key={header.id}>{datum[header.key]}</td>
                    )}
                </tr>
            )
        }
    </tbody>


const isChecked = ({ datum, selected, isEquivalent }) => 
    selected.some(item => isEquivalent(item, datum))


const isPageChecked = ({ data, selected, isEquivalent }) => {

    let result = true
    data.forEach(datum => {
        if (!selected.some(x => isEquivalent(x, datum))) {
            result = false
            return
        }
    })

    return result
}

const toggle = ({ datum, selected, isEquivalent, update, location }) => {
    let newSelected = selected.slice()

    const index = newSelected.findIndex((x) => isEquivalent(x, datum));

    if (index === -1) {
        newSelected.push(datum)
    } else {
        newSelected.splice(index, 1)
    }
    
    update(location, newSelected);
}

const togglePage = ({ data, selected, isEquivalent, update, location, checked }) => {

    let newSelected = []

    if (checked) {

        const itemsInListNotSelected = data.filter(datum => {
            return selected.findIndex(x => isEquivalent(x, datum)) === -1
        })

        newSelected = [...selected, ...itemsInListNotSelected]

    } else {

        newSelected = selected.filter(x => {
            data.findIndex(datum => isEquivalent(x, datum) === -1
            )
        })

    }

    update(location, newSelected)

}

export default ActionableTable

//export default class ActionableTable extends React.Component {

//    constructor(props) {
//        super(props)

//        this.toggle = this.toggle.bind(this)
//        this.togglePage = this.togglePage.bind(this)
//        this.isChecked = this.isChecked.bind(this)
//    }

//    renderHead() {
//        const { headers } = this.props
//        return <thead>
//            <tr>

//                <td>
//                    <input
//                        type="checkbox"
//                        checked={this.isPageChecked()}
//                        onChange={(event) => this.togglePage(event.target.checked)}
//                    />
//                </td>

//                {headers.map(x =>
//                    <td key={x.id}>{x.title}</td>
//                )}
//            </tr>
//        </thead>
//    }

    //renderBody() {
    //    const { data, headers } = this.props

    //    return <tbody>
    //        {
    //            data.map(datum =>
    //                <tr key={datum.id}>

    //                    <td>
    //                        <input
    //                            type="checkbox"
    //                            checked={this.isChecked(datum)}
    //                            onChange={() => {
    //                                this.toggle(datum)
    //                            }}
    //                        />
    //                    </td>


    //                    {headers.map(header =>
    //                        <td key={header.id}>{datum[header.key]}</td>
    //                    )}
    //                </tr>
    //            )
    //        }
    //    </tbody>
    //}

    //render() {
    //    return <table>
    //        {this.renderHead()}
    //        {this.renderBody()}
    //    </table>
    //}

    //isChecked(datum) {
    //    const { selected, isEquivalent } = this.props

    //    return selected.some(item => isEquivalent(item, datum))
    //}

    //isPageChecked() {
    //    const { data, selected, isEquivalent } = this.props

    //    let result = true
    //    data.forEach(datum => {
    //        if (!selected.some(x => isEquivalent(x, datum))) {
    //            result = false
    //            return
    //        }
    //    })

    //    return result
    //}

    //toggle(datum) {
    //    let selected = this.props.selected.slice()
    //    const isEquivalent = this.props.isEquivalent

    //    const index = selected.findIndex((x) => isEquivalent(x, datum));

    //    if (index === -1) {
    //        selected.push(datum)
    //    } else {
    //        selected.splice(index, 1)
    //    }

    //    this.props.update(this.props.location, selected);
    //}

    //togglePage(isChecked) {

    //    const {
    //        data,
    //        selected,
    //        location,
    //        isEquivalent } = this.props

    //    let newSelected = []

    //    if (isChecked) {

    //        const itemsInListNotSelected = data.filter(datum => {
    //            return selected.findIndex(x => isEquivalent(x, datum)) === -1
    //        })

    //        newSelected = [...selected, ...itemsInListNotSelected]

    //    } else {

    //        newSelected = selected.filter(x => {
    //            data.findIndex(datum => isEquivalent(x, datum) === -1
    //            )
    //        })

    //    }

    //    this.props.update(location, newSelected)

    //}

//}




//export default class ActionableTable extends React.Component {

//    constructor(props) {
//        super(props)

//        this.toggle = this.toggle.bind(this)
//        this.togglePage = this.togglePage.bind(this)
//        this.isChecked = this.isChecked.bind(this)        
//    }

//    renderHead() {
//        const { headers } = this.props
//        return <thead>
//            <tr>

//                <td>
//                    <input
//                        type="checkbox"
//                        checked={this.isPageChecked()}
//                        onChange={(event) => this.togglePage(event.target.checked)}
//                    />
//                </td>

//                {headers.map(x =>
//                    <td key={x.id}>{x.title}</td>
//                )}
//            </tr>
//        </thead>
//    }

//    renderBody() {
//        const { data, headers } = this.props

//        return <tbody>
//        {
//            data.map(datum =>
//                <tr key={datum.id}>

//                    <td>
//                        <input
//                            type="checkbox"
//                            checked={this.isChecked(datum)}
//                            onChange={() => {
//                                this.toggle(datum)
//                            }}
//                        />
//                    </td>


//                    {headers.map(header =>
//                        <td key={header.id}>{datum[header.key]}</td>
//                    )}
//                </tr>
//            )
//        }
//        </tbody>
//    }

//    render() {
//        return <table>
//            {this.renderHead()}
//            {this.renderBody()}              
//            </table>           
//    }

//    isChecked(datum) {        
//        const { selected, isEquivalent } = this.props

//        return selected.some(item => isEquivalent(item, datum))
//    }

//    isPageChecked() {
//        const { data, selected, isEquivalent } = this.props

//        let result = true
//        data.forEach(datum => {
//            if (!selected.some( x => isEquivalent(x, datum))) {
//                result = false
//                return
//            }
//        })

//        return result
//    }

//    toggle(datum) {
//        let selected = this.props.selected.slice()
//        const isEquivalent = this.props.isEquivalent

//        const index = selected.findIndex((x) => isEquivalent(x, datum));

//        if (index === -1) {
//            selected.push(datum)
//        } else {
//            selected.splice(index, 1)
//        }

//        this.props.update(this.props.location, selected);
//    }

//    togglePage(isChecked) {

//        const {
//            data,
//            selected,
//            location,
//            isEquivalent } = this.props

//        let newSelected = []

//        if (isChecked) {
            
//            const itemsInListNotSelected = data.filter(datum => {
//                return selected.findIndex(x => isEquivalent(x, datum)) === -1
//            })

//            newSelected = [...selected, ...itemsInListNotSelected]

//        } else {
            
//            newSelected = selected.filter(x => {
//                data.findIndex(datum => isEquivalent(x, datum) === -1
//                )
//            })
            
//        }

//        this.props.update(location, newSelected)

//    }

//}

