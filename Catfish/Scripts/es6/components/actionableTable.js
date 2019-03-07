import React from "react"
import ReactDOM from "react-dom"
import PropTypes from 'prop-types'
import ConditionalRender from './conditionalRender'
import ActionButtons from './actionButtons'
import { range } from '../helpers'

const checkboxStyle = {
    width: "20px"
}

const height40pxStyle = {
    height: "40px"
}

const height50pxStyle = {
    height: "50px"
}

const ActionableTable = (props) => {

    const {
        selected,
        actions,
        data,
        isEquivalent,
        update,
        location,
    } = props



    return (
        <div>
            <div className="row">
                <div className="col-md-12">
                    <table className="table">
                        <tbody>
                            <tr style={height50pxStyle}>
                                <td style={checkboxStyle}>
                                    <input
                                        type="checkbox"
                                        checked={isPageChecked({ data, selected, isEquivalent })}
                                        onChange={(event) => {
                                            const checked = event.target.checked
                                            togglePage({ data, selected, isEquivalent, update, location, checked })
                                        }}
                                    />
                                </td>
                                <td>
                                    <ConditionalRender condition={selected.length > 0}>
                                        <ActionButtons
                                            actions={actions}
                                            payload={selected}
                                        />
                                    </ConditionalRender>
                                </td>
                            </tr>
                        </tbody>
                    </table>

                </div>
            </div>
            <table className="table actionable-table">
                {renderHead(props)}
                {renderBody(props)}            
            </table>
        </div>
        )
}

ActionableTable.propTypes = {
    location: PropTypes.string.isRequired,
    data: PropTypes.array.isRequired,
    selected: PropTypes.array.isRequired,
    headers: PropTypes.array.isRequired,
    update: PropTypes.func.isRequired,
    isEquivalent: PropTypes.func.isRequired,
    maxRows: PropTypes.number,
    actions: PropTypes.array.isRequired
}

ActionableTable.defaultProps = {
    maxRows: 0
}

const renderHead = props => {
    const { headers,
        data,
        selected,
        isEquivalent,
        update,
        location } = props
    return (
        <thead>
            <tr>

                <td style={checkboxStyle}>                   
                </td>

                {headers.map(x =>
                    <td key={x.id}>{x.title}</td>
                )}
            </tr>
        </thead>
        )
}



const getExtraRows = (maxRows, dataLength, headers) => {
    if (maxRows > dataLength + 1) {
        return range(dataLength + 1, maxRows)
            .map(value =>
                <tr key={value} style={height40pxStyle}>

                    <td></td>
                    {headers.map(header =>
                        <td key={header.id}></td>
                    )}
                </tr>
            )
    } 

    return null
}

const renderBody = props => {

    const {
        data,
        headers,
        selected,
        isEquivalent,
        update,
        location,
        maxRows = 0
    } = props

    const restOfTable = getExtraRows(maxRows, data.length, headers)
    
    return (             
        <tbody>
            {
                data.map(datum =>
                    <tr key={datum.id} style={height40pxStyle}>

                        <td style={checkboxStyle}>
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
            { restOfTable }            
        </tbody>
        )
}

const isChecked = ({ datum, selected, isEquivalent }) => 
    selected.some(item => isEquivalent(item, datum))


const isPageChecked = ({ data, selected, isEquivalent }) => {

    if (selected.length == 0) {
        return false
    }

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
    let newSelected = [...selected]

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
            return data.findIndex(datum => isEquivalent(x, datum)) === -1
        })

    }

    update(location, newSelected)

}

export default ActionableTable
