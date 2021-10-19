$(function () {

    let queryStr = window.location.search;
    if (queryStr !== "") {
        $("#freeSearchBlockTerm").val(queryStr.substring(3));

        document.getElementById('freeSearchBlockButton').click(function (event) {
            executeSearch(event);
        });
    }


    //$("#freeSearchBlockForm").submit(function (event) {

    //    /* stop form from submitting normally */
    //    event.preventDefault();

    //    /* get the action attribute from the <form action=""> element */
    //    var $form = $(this),
    //        url = $form.attr('action');

    //    let queryStr = window.location.search;
    //    if (queryStr !== "") {
    //        $("#freeSearchBlockTerm").val(queryStr.substring(3));
    //    }

    //    var searchVal = $('#freeSearchBlockTerm').val();

    //    /* Send the data using post with element id name and name2*/
    //    var posting = $.post(url, {
    //        searchTerm: searchVal
    //    });

    //    /* Alerts the results */
    //    posting.done(function (data) {
    //        //TODO: Check the content of the data to understand it.
    //        //Iterate throgh the list of results and add them as new elements to the search-result div

    //        //Clearing the existing search results, if any
    //        $("#freeSearchBlockResults").empty();
    //        if (data.length == 0) {
    //            $("#freeSearchBlockResults").append($.parseHTML("<div class='alert alert-warning'>No results found</div>"));
    //            return;
    //        }

    //        $.each(data, function (entryIndex, entry) {

    //            //Creating the wrapper div as a DOM element for the result item
    //            var entryWrapper = $.parseHTML("<div class='search-result-entry'>");

    //            //Adding the wrapper to the result - list container
    //            $("#freeSearchBlockResults").append(entryWrapper);

    //            //Building a link to the detailed object if there is a permalink associated with the entry
    //            var linkStart = "";
    //            var linkEnd = "";
    //            if (entry.permalink != undefined && entry.permalink.length > 0) {
    //                linkStart = "<a href='" + entry.permalink + "'>";
    //                linkEnd = "</a>";
    //            }

    //            //Adding the entry title to the wrapper as a child. We need to this for each
    //            //    value of the title because the title can take multiple values
    //            var entryTitleDiv = $.parseHTML("<div class='entry-title'>");
    //            $(entryWrapper).append(entryTitleDiv);
    //            $.each(entry.title, function (titleIndex, titleVal) {
    //                var str = "<span><b>" + linkStart + titleVal + linkEnd + "</b></span>"
    //                $(entryTitleDiv).append($.parseHTML(str));
    //            });

    //            //Go through each highlight snippet and add them to the entry-body section. We include each them
    //            //    in separate divs
    //            var entryBodyDiv = $.parseHTML("<div class='entry-body'>");
    //            $(entryWrapper).append(entryBodyDiv);
    //            $.each(entry.highlights, function (highlightIndex, highlightVal) {
    //                var str = "<div>" + highlightVal + "</div>";
    //                $(entryBodyDiv).append($.parseHTML(str));
    //            });
    //        });
    //    });

    //    posting.fail(function () {
    //        alert('The search failed!');
    //    });
    //});
});


function executeSearch(event) {
    $("#freeSearchBlockForm").submit(function (event) {

        /* stop form from submitting normally */
        event.preventDefault();

        /* get the action attribute from the <form action=""> element */
        var $form = $(this),
            url = $form.attr('action');

        let queryStr = window.location.search;
        if (queryStr !== "") {
            $("#freeSearchBlockTerm").val(queryStr.substring(3));
        }

        var searchVal = $('#freeSearchBlockTerm').val();

        /* Send the data using post with element id name and name2*/
        var posting = $.post(url, {
            searchTerm: searchVal
        });

        /* Alerts the results */
        posting.done(function (data) {
            //TODO: Check the content of the data to understand it.
            //Iterate throgh the list of results and add them as new elements to the search-result div

            //Clearing the existing search results, if any
            $("#freeSearchBlockResults").empty();
            if (data.length == 0) {
                $("#freeSearchBlockResults").append($.parseHTML("<div class='alert alert-warning'>No results found</div>"));
                return;
            }

            $.each(data, function (entryIndex, entry) {

                //Creating the wrapper div as a DOM element for the result item
                var entryWrapper = $.parseHTML("<div class='search-result-entry'>");

                //Adding the wrapper to the result - list container
                $("#freeSearchBlockResults").append(entryWrapper);

                //Building a link to the detailed object if there is a permalink associated with the entry
                var linkStart = "";
                var linkEnd = "";
                if (entry.permalink != undefined && entry.permalink.length > 0) {
                    linkStart = "<a href='" + entry.permalink + "'>";
                    linkEnd = "</a>";
                }

                //Adding the entry title to the wrapper as a child. We need to this for each
                //    value of the title because the title can take multiple values
                var entryTitleDiv = $.parseHTML("<div class='entry-title'>");
                $(entryWrapper).append(entryTitleDiv);
                $.each(entry.title, function (titleIndex, titleVal) {
                    var str = "<span><b>" + linkStart + titleVal + linkEnd + "</b></span>"
                    $(entryTitleDiv).append($.parseHTML(str));
                });

                //Go through each highlight snippet and add them to the entry-body section. We include each them
                //    in separate divs
                var entryBodyDiv = $.parseHTML("<div class='entry-body'>");
                $(entryWrapper).append(entryBodyDiv);
                $.each(entry.highlights, function (highlightIndex, highlightVal) {
                    var str = "<div>" + highlightVal + "</div>";
                    $(entryBodyDiv).append($.parseHTML(str));
                });
            });
        });

        posting.fail(function () {
            alert('The search failed!');
        });
    });
}

function eventFire(el, etype) {
    if (el.fireEvent) {
        el.fireEvent('on' + etype);
    } else {
        var evObj = document.createEvent('Events');
        evObj.initEvent(etype, true, false);
        el.dispatchEvent(evObj);
    }
}