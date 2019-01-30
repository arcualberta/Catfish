import React from "react"
import ReactDOM from "react-dom"
import PropTypes from 'prop-types'
import update from 'immutability-helper'


export default class ActionableTable extends React.Component {

    constructor(props) {
        super(props)

        this.toggle = this.toggle.bind(this)
        this.togglePage = this.togglePage.bind(this)
        this.isChecked = this.isChecked.bind(this)        
    }

    renderHead() {
        const { headers } = this.props
        return <thead>
            <tr>

                <td>
                    <input
                        type="checkbox"
                        checked={this.isPageChecked()}
                        onChange={(event) => this.togglePage(event.target.checked)}
                    />
                </td>

                {headers.map(x =>
                    <td key={x.id}>{x.title}</td>
                )}
            </tr>
        </thead>
    }

    renderBody() {
        const { data, headers } = this.props

        return <tbody>
        {
            data.map(datum =>
                <tr key={datum.id}>

                    <td>
                        <input
                            type="checkbox"
                            checked={this.isChecked(datum)}
                            onChange={() => {
                                this.toggle(datum)
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
    }

    render() {
        return <table>
            {this.renderHead()}
            {this.renderBody()}              
            </table>           
    }

    isChecked(datum) {        
        const { selected, isEquivalent } = this.props

        return selected.some(item => isEquivalent(item, datum))
    }

    isPageChecked() {
        const { data, selected, isEquivalent } = this.props

        let result = true
        data.forEach(datum => {
            if (!selected.some( x => isEquivalent(x, datum))) {
                result = false
                return
            }
        })

        return result
    }

    toggle(datum) {
        let selected = this.props.selected.slice()
        const isEquivalent = this.props.isEquivalent

        const index = selected.findIndex((x) => isEquivalent(x, datum));

        if (index === -1) {
            selected.push(datum)
        } else {
            selected.splice(index, 1)
        }

        this.props.update(this.props.location, selected);
    }

    togglePage(isChecked) {

        const {
            data,
            selected,
            location,
            isEquivalent } = this.props

        let newSelected = []

        if (isChecked) {
            
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

        this.props.update(location, newSelected)

    }

}

