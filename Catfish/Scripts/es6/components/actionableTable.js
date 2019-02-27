import React from "react"
import ReactDOM from "react-dom"
import PropTypes from 'prop-types'

const ActionableTable = (props) => {
  
    return (
        <table className="actionable-table">
            {renderHead({...props})}
            {renderBody({...props})}            
        </table>
        )
}
ActionableTable.propTypes = {
    location: PropTypes.string.isRequired,
    data: PropTypes.array.isRequired,
    selected: PropTypes.array.isRequired,
    headers: PropTypes.array.isRequired,
    update: PropTypes.func.isRequired,
    isEquivalent: PropTypes.func.isRequired
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
