import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';


const ActionButtons = ({ actions, data }) =>
    <div>
        {actions.map((actionable, index) =>
            <button
                key={index}
                onClick={
                    (event) => {
                        { actionable.action(data) }
                    }
                }>
                {actionable.title}
            </button>
        )}
    </div>


export default ActionButtons