import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';

const ActionableDropDownList = (props) =>
{
    return (<select value={props.selectedType}
                    onChange={props.handleChange} 
                    className="dropdown-toggle form-control"
                    >
                    <option key="" value=""></option>

                    {props.data.map(x=>
                         <option key={x.Value}  value={x.Text}>{x.Text}</option>
                     )}
          
                    </select>
                    );
}

ActionableDropDownList.propTypes = {
    handleChange: PropTypes.func.isRequired,
    data: PropTypes.array.isRequired,
    selectedType: PropTypes.string
}


export default ActionableDropDownList