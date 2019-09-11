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

window.startLoading = function (containerPath, timeout, timeoutCallback) {
    var container = $(containerPath);
    var children = container.children(".loading-panel");

    if (!(children[0])) {
        var loadingPanel = $("<div></div>");
        loadingPanel.addClass("loading-panel");
        container.append(loadingPanel);

        var loadingDiv = $("<div></div>");
        loadingPanel.append(loadingDiv);

        var element = $("<div></div>");
        element.addClass("message");
        element.text("Loading...");
        loadingDiv.append(element);

        window.setTimeout(function () { window.stopLoading(containerPath); if (timeoutCallback) { timeoutCallback(containerPath); } }, timeout);
    }
}

function createGuid() {
    function S4() {
        return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
    }

    guid = (S4() + S4() + "-" + S4() + S4() + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();

    return guid;
}

window.stopLoading = function (containerPath) {
    var children = $(containerPath).children(".loading-panel");

    if (children[0]) {
        children.remove();
    }
}