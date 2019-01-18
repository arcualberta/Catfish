var GraphField = function (metadataSet, field) {
    this.metadataSet = metadataSet;
    this.field = field;
}

function GraphPanel(panelId, updateUrl, chartType, xLabel, yLabel, graphTitle, xScale, yScale, xMetadataSet, xField, yMetadataSet, yField, catMetadataSet, catField, isCategoryOptionsField) {
    this.panelId = panelId;

    if (chartType == "Bar") {
        this.graph = new MultiBarChart(xLabel, yLabel, graphTitle, xScale, yScale, $('#' + panelId + ' > svg')[0], $('#' + panelId + ' > .legend')[0]);
    } else {
        this.graph = new MultiLineChart(xLabel, yLabel, graphTitle, xScale, yScale, $('#' + panelId + ' > svg')[0], $('#' + panelId + ' > .legend')[0]);
    }

    this.query = '*:*';
    this.chartType = chartType;
    this.updateUrl = updateUrl;
    this.isCategoryOptionsField = isCategoryOptionsField;

    this.x = new GraphField(xMetadataSet, xField);
    this.y = new GraphField(yMetadataSet, yField);
    this.category = new GraphField(catMetadataSet, catField);

    this.isLoaded = false;
    this.isUpdating = false;
    this.hasUpdate = false;

    var _this = this;

    var updateParameters = function (e) {
        _this.updateParameters(e);
    }

    window.addEventListener('updatedparams', updateParameters);
}
GraphPanel.prototype.updateParameters = function (e) {
    var params = e.detail;
    var triggerUpdate = !this.isLoaded;

    if ('q' in params) {
        triggerUpdate = true;
        this.query = params['q'];
    }

    if (triggerUpdate) {
        this.isLoaded = true;
        this.updateChart();
    }
}
GraphPanel.prototype.updateChart = function () {
    if (this.isUpdating) {
        this.hasUpdate = true;
        return;
    }

    this.isUpdating = true;
    var _this = this;
    var panelId = "#" + _this.panelId;
    var graph = _this.graph;
    var chartType = _this.chartType;

    window.startLoading(panelId, 60000);

    var data = {
        q: _this.query,
        xMetadataSet: _this.x.metadataSet,
        yMetadataSet: _this.y.metadataSet,
        xField: _this.x.field,
        yField: _this.y.field
    }

    if (_this.category.metadataSet != null && _this.category.field != null) {
        data.catMetadataset = _this.category.metadataSet;
        data.catField = _this.category.field;
        data.isCatOptionsIndex = _this.isCatOptionsIndex;
    }

    $.ajax({
        type: 'GET',
        url: _this.updateUrl,
        dataType: 'json',
        data: data,
        success: function (response) {
            if (chartType === "Bar") {
                var barOption = $(paneId + " .barchartOption")[0];
                barOption.style.display = "block";
            }
            else {
                var barOption = $(panelId + " .barchartOption")[0];
                barOption.style.display = "none";
            }

            var parsedData = graph.parseDataSF(response);
            //graph.clear();
            graph.drawChart(parsedData);
        },
        error: function (err) {
            console.log(err);
        },
        complete: function (err) {
            window.stopLoading(panelId);

            // Prevent multiple updates from being triggered.
            _this.isUpdating = false;
            if (_this.hasUpdate) {
                _this.hasUpdate = false;
                _this.updateChart();
            }
        }

    });
}

//Parse data into key-value pairs
var Graph = function () {
    Graph.prototype.init.apply(this, arguments);
}
Graph.prototype.init = function (xLabel, yLabel, graphTitle, xScale, yScale, svgElement, legendElement) {
    this.XLabel = xLabel;
    this.YLabel = yLabel;
    this.GraphTitle = graphTitle;
    this.XScale = xScale;
    this.YScale = yScale;
    this.Svg = svgElement;
    this.Legend = legendElement;
    
    this.generateBase.apply(this, arguments);
}
Graph.prototype.generateBase = function () {
    //This will be overridden by the other graph types and used as the basis for each graph.
}
Graph.prototype.clear = function () {
    $(this.Svg).empty();
    $(this.Legend).empty();
}
Graph.prototype.parseDataSF = function (data) {
    var arr = [];
    var XScale = this.XScale;
    var YScale = this.YScale;

    for (var i in data) {
        arr.push({
            year: data[i].YValue / XScale, //YValue = Year
            value: +(data[i].XValue / YScale),//(Math.log(+data[i].XValue)), //XValue = Amount 
            category: data[i].Category
        });
    }

    return arr;
}
Graph.prototype.selectSvg = function () {
    // Adds the svg canvas
    var svg = d3.select(Svg).attr("viewBox", "0 0 600 400");
    return svg;
}
Graph.prototype.drawChart = function (data) {
    //This will be overridden by the other graph types and used as the basis for each graph.
}

var MultiLineChart = function () {
    Graph.prototype.init.apply(this, arguments);
}
MultiLineChart.prototype = Object.create(Graph.prototype);
{
    function drawAxis(g1, title, width, height, margin, XLabel, YLabel, x, y) {
        // Add the X Axis
        this.xAxis = g1.append("g")
            .attr("class", "axis axis-y")
            .attr("transform", "translate(0," + height + ")");

        this.xAxis.append("text").attr("fill", "#1f77b4").attr("y", 35).attr("x", width / 2).attr("text-anchor", "end").text(XLabel);

        // Add the Y Axis
        this.yAxis = g1.append("g")
            .attr("class", "axis axis-x");

        this.yAxis.append("text").attr("fill", "#1f77b4")
            .attr("transform", "rotate(-90)")
            .attr("y", 0 - (margin.left / 2))
            .attr("x", 0 - (height / 2))
            .attr("dy", "0.17em")
            .attr("text-anchor", "middle")
            .text(YLabel);
            
        //adding title
        g1.append("text").attr("fill", "#1f77b4")
            .attr("x", (width / 2))
            .attr("y", 0 - (margin.top / 2))
            .attr("text-anchor", "middle")
            .style("font-size", "16px")
            .style("text-decoration", "bold")
            .text(title);

        updateAxis(this.xAxis, this.yAxis, x, y);
    }

    function updateAxis(xAxis, yAxis, x, y) {
        xAxis.call(d3.axisBottom(x));
            

        yAxis.call(d3.axisLeft(y));
    }

    function setPathProperties(path, lineFunction, transition) {
        path.transition(transition)
            .attr("class", function (d, i) { return "line item-" + i; })
            .style("stroke", function (d) { // Add the colours dynamically
                return d.color;
            })
            .attr("d", function (d) {
                return lineFunction(d.values);
            })
            .attr("id", function (d, i) { return "line_" + i; });
    }

    function updateLines(chartArea, dataNest, lineFunction, transition) {
        var data = dataNest.filter(function (d) {
            return d.checked;
        });

        var lines = chartArea.selectAll("path.line").data(data);

        // Remove unused lines
        lines.exit().style("opacity", 1.0).transition(transition).style("opacity", 0.0).remove();
        
        // Update old elements
        setPathProperties(lines, lineFunction, transition);
    
        // Add new elements
        path = lines.enter().append("path");
        setPathProperties(path, lineFunction, transition);

        if (path.node()) {
            var totalLength = path.node().getTotalLength();

            path.attr("stroke-dasharray", totalLength + " " + totalLength)
                .attr("stroke-dashoffset", 0);
            //.attr("stroke-dashoffset", totalLength).transition().duration(100).ease(d3.easeCubicOut)
        }            
    }

    function setLegendItemProperties(div, dataNest, graph) {
        div.attr("class", function (d, i) {
                return "legend-item item-" + i;
            })
            .style("color", function (d, i) {
                return d.color;
            });

        div.selectAll("input").attr("id", function (d, i) {
                return "checkbox_" + i;
            })
            .attr("type", "checkbox")
            .attr("checked", "checked")
            .on("change", function (d, i, input) {
                d.checked = input[i].checked;

                /*if (input[i].checked) {
                    $("#line_" + i).show();
                } else {
                    $("#line_" + i).hide();
                }*/

                graph.update(dataNest);
            });

        div.selectAll("span").text(function (d, i) { return " " + d.key; });
    }

    function updateLegend(divLegend, dataNest, graph) {
        var legendCategories = divLegend.selectAll("div.legend-item").data(dataNest);
        
        // Remove unused categories
        legendCategories.exit().remove();

        // Update old elements
        setLegendItemProperties(legendCategories, dataNest, graph);

        // Add new elements
        var div = legendCategories.enter().append("div");
        div.append("input");
        div.append("span");
        setLegendItemProperties(div, dataNest, graph);
    }

    MultiLineChart.prototype.generateBase = function () {
        // Adds the svg canvas
        var svg = d3.select(this.Svg).attr("viewBox", "0 0 600 400");

        var margin = { top: 30, right: 20, bottom: 70, left: 70 };
        var width = 600 - margin.left - margin.right;
        var height = 400 - margin.top - margin.bottom;

        //// Set the ranges
        this.x = d3.scaleTime().range([0, width]);
        this.y = d3.scaleLinear().range([height, 0]);

        this.svgD3 = svg;
        this.g1 = svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + " )");
        this.divLegend = d3.select(this.Legend);
        this.chartArea = svg.append("g")
            .attr("transform", "translate(" + margin.left + "," + margin.top + " )");

        // Draw the base axis
        drawAxis.call(this, this.g1, this.GraphTitle, width, height, margin, this.XLabel, this.YLabel, this.x, this.y);
    }

    MultiLineChart.prototype.clear = function () {
        //$(this.Svg).empty();
        //$(this.Legend).empty();
    }

    MultiLineChart.prototype.update = function (dataNest) {
        var x = this.x;
        var y = this.y;
        var maxY = 0;
        var transition = d3.transition().duration(100).ease(d3.easeCubicOut);

        // set the colour scale
        var colorFunction = d3.scaleOrdinal(d3.schemeCategory10);

        // Define the line
        var lineFunction = d3.line()
            .x(function (d) {
                return x(d.year);
            })
            .y(function (d) {
                return y(d.value);
            });

        // Setup the data
        dataNest.forEach(function (d, i) {
            d.color = colorFunction(d.key);

            if (d.checked == undefined) {
                d.checked = true;
            }

            d.values.forEach(function (d, i) {
                if (maxY < d.value) {
                    maxY = d.value;
                }
            });
        });

        // Max y
        y.domain([0, maxY]);

        //legendSpace = width / dataNest.length; // spacing for the legend
        updateAxis(this.xAxis, this.yAxis, x, y);
        updateLegend(this.divLegend, dataNest, this);
        updateLines(this.chartArea, dataNest, lineFunction);

    }

    MultiLineChart.prototype.drawChart = function (data) {
        var parseTime = d3.timeParse("%Y");
        var x = this.x;
        var y = this.y;

        // Get the data   
        data.forEach(function (d) {
            d.year = parseTime(d.year);
            d.value = +d.value;
        });
        
        //get min/max year
        var minX = d3.min(data, function (d) { return d.year; });
        var maxX = d3.max(data, function (d) { return d.year; });

        // Scale the range of the data 
        x.domain(d3.extent(data, function (d) { return d.year; }));
        y.domain([0, d3.max(data, function (d) { return d.value; })]);

        // Nest the entries by category
        var dataNest = d3.nest()
            .key(function (d) { return d.category; })
            .entries(data);

        this.update(dataNest);
    }
}

/*
   based on stacked/group bar by bl.ocks.org/mbostock/3943967
*/
var MultiBarChart = function () {
    Graph.prototype.init.apply(this, arguments);
}
MultiBarChart.prototype = Object.create(Graph.prototype);
{
    MultiBarChart.prototype.generateBase = function () {
        //This will be overridden by the other graph types and used as the basis for each graph.
    }
    MultiBarChart.prototype.drawChart = function (data) {
        var svgWidth = 1100, svgHeight = 500;
        var cats = [];
        var years = [];
        var nested = d3.nest()
            .key(function (d) { return d["category"]; })
            .entries(data)
        var i = 0;
        nested.forEach(function (d) {
            if ($.inArray(d.key, cats) < 0) {
                cats.push(d.key);
                i++;
            }
        });

        data.forEach(function (d) {
            if ($.inArray(d.year, years) < 0) {
                years.push(d.year);
            }
        });
        var n = nested.length, //number of layers
            m = years.length, //number of samples per layer

            stack = d3.stack().keys(cats);
        stack.value(function (d, key) {
            var v = 0;
            d.values.forEach(function (l) {
                if (l.category === key)
                    v = l.value;

            })
            return v;
        });


        var myData = d3.nest().key(function (d) { return d.year }).entries(data);//group data by x (i.e year)

        var layers = stack(myData);


        var yGroupMax = d3.max(layers, function (layer) {
            return d3.max(layer, function (d) {
                var v = d[1] - d[0];
                return v;
            });
        }),
            yStackMax = d3.max(layers, function (layer) {
                return d3.max(layer, function (d) {
                    return d[1];
                });
            });

        var margin = { top: 40, right: 10, bottom: 20, left: 10 },
            width = 960 - margin.left - margin.right,
            height = 500 - margin.top - margin.bottom;

        var x = d3.scaleBand()
            .domain(years.map(function (d) { return d; }))//display year
            .rangeRound([0, width]).paddingInner(0.05).align(0.01);


        var y = d3.scaleLinear()
            .domain([0, yStackMax])
            .range([height, 0]);

        var color = d3.scaleOrdinal(d3.schemeCategory10);

        var xAxis = d3.axisBottom(x)
            .tickSize(0)
            .tickPadding(6);
        var yAxis = d3.axisLeft(y).tickPadding(6);


        var svg = d3.select(Svg)
            .attr("width", svgWidth)
            .attr("height", height + margin.top + margin.bottom);
        var g1 = svg.append("g")
            .attr("transform", "translate(" + (margin.left + 50) + "," + margin.top + ")");

        var layer = g1.selectAll(".layer")
            .data(layers)
            .enter().append("g")
            .attr("class", "layer")
            .attr("layernum", function (d, i) {
                return i;
            })
            .style("fill", function (d, i) {
                return color(d.key);
            });

        var rect = layer.selectAll("rect")
            .data(function (d) {
                return d;
            })
            .enter().append("rect")
            .attr("x", function (d, i) {
                return x(d.data.key);
            })
            .attr("y", height)
            .attr("width", x.bandwidth() - 1)
            .attr("height", 0)
            .on("mouseover", function () { tooltip.style("opacity", 1) })
            .on("mouseout", function () { tooltip.style("opacity", 0) })
            .on("mousemove", function (d) {
                var xPos = d3.mouse(this)[0] + 15;
                var yPos = d3.mouse(this)[1] - 15;
                var f = d3.format(".2f");
                var amt = f(d[1] - d[0]);
                tooltip.attr("transform", "translate(" + xPos + "," + yPos + ")");
                tooltip.style("background", "White");
                tooltip.select("text").text(amt);
            })
            ;
        //.call(addToolTip);

        rect.transition()
            .delay(function (d, i) {
                return i * 10;
            })
            .attr("y", function (d) {
                return y(d[1]);
            })
            .attr("height", function (d) {
                return y(d[0]) - y(d[1]);
            })

        //add x axis
        g1.append("g")
            .attr("class", "x axis")
            .attr("transform", "translate(2," + (height) + ")")
            .attr("y", height + margin.bottom)
            .call(xAxis).selectAll("text").attr("transform", "rotate(35)");
        //add y axis
        g1.append("g")
            .attr("class", "y axis")
            .attr("transform", "translate(0, 0)")
            // .call(d3.axisRight(y));
            .call(d3.axisLeft(y));

        var legend = g1.append("g")
            .attr("font-family", "sans-serif")
            .attr("font-size", 10)
            .attr("text-anchor", "end")
            .selectAll("g")
            .data(cats.reverse())
            .enter().append("g")
            .attr("transform", function (d, i) { return "translate(-800," + i * 20 + ")"; });
        legend.append("text")
            .attr("x", width - 24)
            .attr("y", 9.5)
            .attr("dy", "0.32em")
            .text(function (d) {
                return d;
            });
        legend.append("rect")
            .attr("x", width - 19)
            .attr("width", 19)
            .attr("height", 19)
            .attr("fill", color);

        var tooltip = g1.append("g")
            .attr("class", "tooltip");
        // .style("display", "none");
        tooltip.append("rect").attr("width", 60)
            .attr("height", 20)
            .attr("fill", "black")
            .style("opacity", 0.7);

        tooltip.append("text").attr("x", 30)
            .attr("dy", "1.2em")
            .style("text-anchor", "middle")
            .style("text-align", "center")
            .attr("font-size", "12px")
            .attr("fill", "white")
            .attr("font-weight", "bold");

        legend.selectAll("input").on("change", change);

        function change() {
            if (this.value === "grouped")
            { transitionGrouped(); }
            else { transitionStacked(); }
        }

        function transitionGrouped() {
            y.domain([0, yGroupMax]);

            rect.transition()
                .duration(500)
                .delay(function (d, i) { return i * 10; })
                .attr("x", function (d, i) {
                    var j = d3.select(this.parentNode).attr("layernum");
                    return x(d.data.key) + (x.bandwidth() - 1) / n * j;
                })
                .attr("width", (x.bandwidth() - 1) / n)
                .transition()
                .attr("y", function (d) { return height - (y(d[0]) - y(d[1])); })
                .attr("height", function (d) { return (y(d[0]) - y(d[1])); });
        }

        function transitionStacked() {
            y.domain([0, yStackMax]);

            rect.transition()
                .duration(500)
                .delay(function (d, i) { return i * 10; })
                .attr("y", function (d) { return y(d[1]); })
                .attr("height", function (d) {

                    return y(d[0]) - y(d[1]);

                })
                .transition()
                .attr("x", function (d, i) {
                    return x(d.data.key);
                })
                .attr("width", x.bandwidth() - 1);

        }

    }
}

