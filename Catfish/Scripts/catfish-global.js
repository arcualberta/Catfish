{
    var currentUrl = new URL(window.location.href);

    function updateUrl(parameters) {
        for (var param in parameters) {
            if (parameters.hasOwnProperty(param)) {
                currentUrl.searchParams.set(param, parameters[param]);
            }
        }

        console.log(currentUrl);
    }

    function triggerEvent(parameters) {
        var event = new CustomEvent('updatedparams', { parameters: parameters });

        window.dispatchEvent(event);
    }

    window.updateUrlParameters = function (parameters) {
        updateurl(parameters);
        triggerEvent(parameters);
    }
}