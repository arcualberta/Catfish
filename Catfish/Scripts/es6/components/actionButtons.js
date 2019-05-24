import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';

const ActionButtons = (props) =>
    <div className="action-buttons">
        {props.actions.map((actionable, index) =>
            
            <button
                key={index}
                id={actionable.id}
                onClick={
                    (event) => {
                        { actionable.action(props.payload) }
                    }
                 } >
                <div dangerouslySetInnerHTML= {{__html: actionable.title}} />

            </button>
        )}
    </div>

ActionButtons.propTypes = {
    //actions: PropTypes.arrayOf(PropTypes.instanceOf(Actionable)).isRequired,
    actions: PropTypes.arrayOf(
    PropTypes.shape({
            title: PropTypes.string.isRequired,
            action: PropTypes.func.isRequired
        })
    ).isRequired,
    //payload: PropTypes.object
}

export default ActionButtons