import React from "react";
import PropTypes from 'prop-types';

const renderBody = ({ data, listKey, isChecked, toggle}) =>
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

const renderHead = ({ data, listKey, areAllChecked, toggleAll}) =>
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
                <table>
                    {renderHead({ data, listKey, areAllChecked, toggleAll })}              
                    {renderBody({ data, listKey, isChecked, toggle })}
                </table>
            </div>
        );
    }


ActionableTable.propTypes = {
    listKey: PropTypes.string.isRequired,
    data: PropTypes.object.isRequired,
    toggle: PropTypes.func.isRequired,
    isChecked: PropTypes.func.isRequired,
    toggleAll: PropTypes.func.isRequired,
    areAllChecked: PropTypes.func.isRequired,
}


export default ActionableTable