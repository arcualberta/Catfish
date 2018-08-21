// This event allows functions to listen for url parameter changes.
// Once the page is loaded, this event is fired with all page parameters available.
{
    var currentUrl = new URL(window.location.href);

    function updateUrl(parameters) {
        for (var param in parameters) {
            if (parameters.hasOwnProperty(param)) {
                currentUrl.searchParams.set(param, parameters[param]);
            }
        }

        var title = document.getElementsByTagName("title")[0].innerHTML;
        window.history.replaceState(parameters, title, currentUrl.href);
    }

    function triggerEvent(parameters) {
        var event = new CustomEvent('updatedparams', { detail: parameters });

        window.dispatchEvent(event);
    }

    window.updateUrlParameters = function (parameters) {
        updateUrl(parameters);
        triggerEvent(parameters);
    };

    $(document).ready(function () {
        var result = {};
        var keys = currentUrl.searchParams.keys();
        var key;

        while (!(key = keys.next()).done) {
            result[key.value] = currentUrl.searchParams.get(key.value);
        }

        window.updateUrlParameters(result);
    });
}