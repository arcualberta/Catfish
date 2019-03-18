import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';

const ActionableInputField = props =>
    <input type="text"
        placeholder={props.placeholder}
        onChange={props.handleChange}
        className="actionable-input-field form-control"
        id={props.id}
    />
    
ActionableInputField.propTypes = {
    handleChange: PropTypes.func.isRequired,
    placeholder: PropTypes.string
}

ActionableInputField.defaultProps = {
    placeholder: ""
}

export default ActionableInputField