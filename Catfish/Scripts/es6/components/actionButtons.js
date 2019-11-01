import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';

const ActionButtons = (props) =>
    <div className="action-buttons">
        {props.actions.map((actionable, index) =>{
            const{
                action,
                title,
                id,
                disabled=()=>false //passing value as function
            }=actionable

            return(
                     <button key={index}
                         id={id}  disabled={disabled()} //to disabled the button
                         onClick={
                               (event) => {
                                    { action(props.payload) }
                                     }
                               } >
                           <div dangerouslySetInnerHTML= {{__html: title}} />

                     </button>
                )

         }
            
           
       )}
    </div>

ActionButtons.propTypes = {
    //actions: PropTypes.arrayOf(PropTypes.instanceOf(Actionable)).isRequired,
    actions: PropTypes.arrayOf(
    PropTypes.shape({
            title: PropTypes.string.isRequired,
            action: PropTypes.func.isRequired,
            disabled:PropTypes.boolean
        })
    ).isRequired,
    
    //payload: PropTypes.object
}

export default ActionButtons