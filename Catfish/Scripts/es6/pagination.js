import React from "react";
import ReactDOM from "react-dom";


const Pagination = ({
    currentPage = 1,
    totalItems = 1,
    itemsPerPage = 10,
    nextPage,
    prevPage,
    goToPage,
    selectPage}) => {

    /*
    << Prev 1 2 3 4 5 6 7 8 9 10 Next >>
    */



    return (
        <div>
            <div>{currentPage}</div>
            <div>{totalItems}</div>
            <div>{itemsPerPage}</div>
        </div>
        )
}

export default Pagination