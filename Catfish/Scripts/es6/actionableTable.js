import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';


const RenderBody = ({ data, listKey, isChecked, toggle}) =>
    <tbody>
        {data.entities.map(datum =>
            <tr key={datum.id}>
                <td>
                    <input
                        type="checkbox"
                        checked={isChecked(listKey, datum.id)}
                        onChange={() => {
                            toggle(listKey, datum.id)
                        }}
                    />
                </td>
                {data.headers.map(header =>
                    <td key={header.id}>{datum[header.key]}</td>
                )}
            </tr>
        )}
    </tbody>

const RenderHead = ({ data, listKey, areAllChecked, toggleAll}) =>
    <thead>
        <tr>
            <th>
                <input
                    type="checkbox"
                    checked={areAllChecked(listKey)}
                    onChange={(event) => {
                        toggleAll(listKey,
                            event.currentTarget.checked)
                    }}
                />
            </th>
            {data.headers.map(header =>
                <th key={header.id}>
                    {header.title}
                </th>
            )}
        </tr>
    </thead>

const RenderActions = ({ actions, selected }) =>
    <div>
        {actions.map((actionable, index) =>
            <button
                key={index}
                onClick={
                    (event) => {
                        { actionable.action(selected) }
                    }
                }>
                {actionable.title}
            </button>
        )}
    </div>

const ActionableTable = ({
    listKey, 
    data,
    toggle,
    isChecked,
    toggleAll,
    areAllChecked,
    actions
}) => {
        const selected = data.selected
        return (
            <div>
                
                {RenderActions({actions, selected})}

                <table>
                    {RenderHead({ data, listKey, areAllChecked, toggleAll })}              
                    {RenderBody({ data, listKey, isChecked, toggle })}
                </table>
            </div>
        );
    }



//export default class ActionableTable extends React.Component {

//    constructor(props) {
//        super(props)
//    }

//    render() {

//        const headers = this.props.data.headers;
//        const listKey = this.props.listKey;

//        return (
//            <div>
//                <div>{this.props.data.title}</div>
//                <div>
//                    {this.props.actions.map((action, index) => 
//                        <button
//                            key={index}
//                            onClick={
//                                (event) => {
//                                    { action.action(this.props.data.selected) }
//                                }
//                            }>
//                            {action.title}
//                        </button>
//                    )}
//                </div>

//                <table>
//                    <thead>
//                        <tr>
//                        <th>
//                        <input
//                            type="checkbox"
//                            checked={this.props.areAllChecked(listKey)}
//                            onChange={(event) => {
//                                this.props.toggleAll(listKey,
//                                    event.currentTarget.checked)
//                            }}
//                        />  
//                        </th>
//                        {headers.map(header =>
//                            <th key={header.id}>
//                                {header.title}
//                            </th>
//                        )}
//                        </tr>
//                    </thead>
//                    <tbody>
                    
//                        {this.props.data.entities.map(datum =>
//                            <tr key={datum.id}>
//                                <td>
//                                    <input
//                                        type="checkbox"
//                                        checked={this.props.isChecked(listKey, datum.id)}
//                                        onChange={() => {
//                                            this.props.toggle(listKey, datum.id)
//                                        }}
//                                    />
//                                </td>
//                                {headers.map(header =>
//                                    <td key={header.id}>{datum[header.key]}</td>    
//                                )}                            
//                            </tr>
//                        )}
//                    </tbody>
//                </table>
//            </div>
//        );
//    }

//}

ActionableTable.propTypes = {
    listKey: PropTypes.string.isRequired,
    data: PropTypes.object.isRequired,
    toggle: PropTypes.func.isRequired,
    isChecked: PropTypes.func.isRequired,
    toggleAll: PropTypes.func.isRequired,
    areAllChecked: PropTypes.func.isRequired,
    actions: PropTypes.array.isRequired
}


export default ActionableTable