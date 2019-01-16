import React from "react";
import ReactDOM from "react-dom";
import PropTypes from 'prop-types';

export default class ActionableEntityList extends React.Component {

    constructor(props) {
        super(props)
    }

    render() {
        return (
            <table>
                <thead>
                </thead>
                <tbody>
                    "test"
                </tbody>
            </table>
        );
    }

}

ActionableEntityList.proptypes = {
    name: PropTypes.string.isRequired,
    selected: PropTypes.array.isRequired,
    entities: PropTypes.array.isRequired
}
