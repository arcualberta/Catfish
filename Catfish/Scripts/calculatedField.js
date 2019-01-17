var CalculatedField = function (panelId, getDataUrl, loadingText, prefix, decimalPlaces, statMode, metadataSet, field, groupByMetadataSet, groupByField) {
    this.query = '*:*';
    this.isLoaded = false;
    this.url = getDataUrl;

    this.prefix = prefix;
    this.decimalPlaces = decimalPlaces;
    this.panelId = panelId;
    this.loadingText = loadingText;
    this.statMode = statMode;
    this.metadataSet = metadataSet;
    this.field = field;
    this.groupByMetadataSet = groupByMetadataSet;
    this.groupByField = groupByField;

    var _this = this;

    var updateParameters = function (e) {
        _this.updateParameters(e);
    }

    window.addEventListener('updatedparams', updateParameters);
}
CalculatedField.prototype.updateParameters = function (e) {
    var params = e.detail;
    var triggerUpdate = !this.isLoaded;

    if ('q' in params) {
        triggerUpdate = true;
        this.query = params['q'];
    }

    if (triggerUpdate) {
        this.isLoaded = true;
        this.updatePanel();
    }
}
CalculatedField.prototype.updatePanel = function () {
    var _this = this;
    var panel = $('#' + _this.panelId + '_Result');

    panel.text(this.loadingText);

    $.ajax({
        type: 'GET',
        url: _this.url,
    dataType: 'json',
        data: {
            q: _this.query,
            statMode: _this.statMode,
            selectedFieldMetadataSet: _this.metadataSet,
            selectedField: _this.field,
            selectedGroupByFieldMetadataSet: _this.groupByMetadataSet,
            selectedGroupByField: _this.groupByField
        },
        success: function (result) {
            _this.handleStatsResult(panel, result);
        },
        error: function () {
            panel.text('error');
        }
    });
}
CalculatedField.prototype.handleStatsResult = function(panel, result){
    var _this = this;

    panel.text(_this.prefix + result.toLocaleString('en-US', {
            minimumFractionDigits: _this.decimalPlaces,
            maximumFractionDigits: _this.decimalPlaces
    })); // TODO: get locale.
}