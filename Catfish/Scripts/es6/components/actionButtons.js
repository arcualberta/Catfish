import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';


const ActionButtons = ({ actions, payload }) =>
    <div>
        {actions.map((actionable, index) =>
            <button
                key={index}
                onClick={
                    (event) => {
                        { actionable.action(payload) }
                    }
                }>
                {actionable.title}
            </button>
        )}
    </div>

ActionButtons.propTypes = {
    actions: PropTypes.array.isRequired,
    //payload: PropTypes.object
}

export default ActionButtons