import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';

const ConditionalRender = (props) => {
    let content 

    if (props.condition == true) {
        content = props.children
    }

    return <div className="conditional-render">{content}</div>
}

export default ConditionalRender