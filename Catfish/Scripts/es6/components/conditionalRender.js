import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';

const ConditionalRender = (props) => {
    if (props.condition == true) {
        return <div>{ props.children }</div>
    } else {
        return <div></div>
    }
}

export default ConditionalRender