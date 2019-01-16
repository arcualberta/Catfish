//javascript
var GraphField = function (metadataSet, field) {
    this.metadataSet = metadataSet;
    this.field = field;
}

function GraphPanel(panelId, updateUrl, chartType, xLabel, yLabel, graphTitle, xScale, yScale, xMetadataSet, xField, yMetadataSet, yField, catMetadataSet, catField, isCategoryOptionsField) {
    this.panelId = panelId;
    this.graph = new Graph(xLabel, yLabel, graphTitle, xScale, yScale, $('#' + panelId + ' > svg')[0], $('#' + panelId + ' > .legend')[0]);
    this.isLoaded = false;
    this.query = '*:*';
    this.chartType = chartType;
    this.updateUrl = updateUrl;
    this.isCategoryOptionsField = isCategoryOptionsField;

    this.x = new GraphField(xMetadataSet, xField);
    this.y = new GraphField(yMetadataSet, yField);
    this.category = new GraphField(catMetadataSet, catField);

    var _this = this;

    var updateParameters = function(e) {
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
        this.updateChart();
    }
}
GraphPanel.prototype.updateChart = function () {
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
            window.stopLoading(panelId);
            graph.clear();
            graph["drawChartMulti" + chartType](parsedData);
        },
        error: function (err) {
            console.log(err);
            window.stopLoading(panelId);
        }
    });
}

//Parse data into key-value pairs
var Graph = function (xLabel, yLabel, graphTitle, xScale, yScale, svgElement, legendElement) {
    var XLabel = xLabel;
    var YLabel = yLabel;
    var GraphTitle = graphTitle;
    var XScale = xScale;
    var YScale = yScale;
    var Svg = svgElement;
    var Legend = legendElement;

    this.clear = function () {
        $(Svg).empty();
        $(Legend).empty();
    }

    this.parseDataSF = function(data) {
        var arr = [];
        for (var i in data) {
            arr.push({
                year: data[i].YValue / XScale, //YValue = Year
                value: +(data[i].XValue / YScale),//(Math.log(+data[i].XValue)), //XValue = Amount 
                category: data[i].Category
            });
        }

        return arr;
    }
    
    function selectSvg() {
        var margin = { top: 30, right: 20, bottom: 70, left: 70 },
            width = 600 - margin.left - margin.right,
            height = 400 - margin.top - margin.bottom;

        // Set the ranges
        var x = d3.scaleTime().range([0, width]);
        var y = d3.scaleLinear().range([height, 0]);

        // Adds the svg canvas
        var svg = d3.select(Svg).attr("viewBox", "0 0 600 400");
        return svg;
    }

    this.drawChartMultiLine = function(data) {

        var margin = { top: 30, right: 20, bottom: 70, left: 70 },
            width = 600 - margin.left - margin.right,
            height = 400 - margin.top - margin.bottom;
        
        // Set the ranges
        var x = d3.scaleTime().range([0, width]);
        var y = d3.scaleLinear().range([height, 0]);

        // Adds the svg canvas
        var svg = d3.select(Svg).attr("viewBox", "0 0 600 400");

        var g1 = svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + " )");
        
        var divLegend = d3.select(Legend);

        var parseTime = d3.timeParse("%Y");

        // Get the data   
        data.forEach(function (d) {
            d.year = parseTime(d.year);
            d.value = +d.value;
        });

        // Define the line
        var lineFunction = d3.line()
            .x(function (d) {
                return x(d.year);
            })
            .y(function (d) {
                return y(d.value);
            });

        //get min/max year
        minX = d3.min(data, function (d) { return d.year; });
        maxX = d3.max(data, function (d) { return d.year; });

        // Scale the range of the data 
        x.domain(d3.extent(data, function (d) { return d.year; }));
        y.domain([0, d3.max(data, function (d) { return d.value; })]);

        // Nest the entries by category
        var dataNest = d3.nest()
            .key(function (d) { return d.category; })
            .entries(data);

        // set the colour scale
        var color = d3.scaleOrdinal(d3.schemeCategory10);

        function redrawChart(dataNest, g1) {
            dataNest.forEach(function (d, dIndex) {
                if (!d.checked) return;

                var path = g1.append("path")
                    .attr("class", "line item-" + dIndex)
                    .style("stroke", function () { // Add the colours dynamically
                        return d.color;
                    })
                    .attr("d", lineFunction(d.values))
                    .attr("id", "line_" + dIndex);

                var totalLength = path.node().getTotalLength();
                path.attr("stroke-dasharray", totalLength + " " + totalLength)
                    .attr("stroke-dashoffset", totalLength).transition().duration(100).ease(d3.easeCubicOut)
                    .attr("stroke-dashoffset", 0);

                path.exit().remove();
            });
        }

        function drawAxis(g1) {
            // Add the X Axis
            g1.append("g")
                .attr("class", "axis axis-y")
                .attr("transform", "translate(0," + height + ")")
                .call(d3.axisBottom(x))
                .append("text").attr("fill", "#1f77b4").attr("y", 35).attr("x", width / 2).attr("text-anchor", "end").text(XLabel);

            // Add the Y Axis
            g1.append("g")
                .attr("class", "axis axis-x")
                .call(d3.axisLeft(y))
                .append("text").attr("fill", "#1f77b4")
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

                .text(GraphTitle);
        }

        function update(dataUpdate) {
            g1.remove();
            g1 = svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + " )");

            var new_max = 0;
            dataUpdate.forEach(function (dt) {
                if (dt.checked) {
                    var check = d3.max(dt.values, function (d) {
                        return d.value;
                    });

                    if (check > new_max) { new_max = check; }
                }
            });

            y.domain([0, new_max]);

            drawAxis(g1);
            redrawChart(dataNest, g1);
        }

        dataNest.forEach(function (d, i) {
            d.checked = true;
            d.color = color(d.key);
            d.id = d.key;

            //Add the legend
            var divCategory = divLegend.append("div")
                .attr("class", "legend-item item-" + i)    // style the legend
                .style("color", function () { // Add the colours dynamically
                    return d.color;
                });

            divCategory.append("input")
                .attr("id", "checkbox_" + i)
                .attr("type", "checkbox")
                .attr("checked", "checked")
                .on("change", function (checkData, i, input) {
                    d.checked = input[i].checked;

                    if (input[i].checked) {
                        $("#line_" + i).show();
                    } else {
                        $("#line_" + i).hide();
                    }
                    update(dataNest);

                });

            divCategory.append("span")
                .text(" " + d.key);
        });

        function drawLegend(d) {
            var divCategory = divLegend.append("div")
                //.attr("x", (legendSpace / 2) + i * legendSpace)  // space legend
                // .attr("y", height + (margin.bottom / 2) + 5)
                .attr("class", "legend-item")    // style the legend
                .style("color", function () { // Add the colours dynamically
                    return d.color;
                });

            divCategory.append("input")
                .attr("id", "checkbox_" + d.id)
                .attr("type", "checkbox")
                .attr("checked", "checked")

                .on("change", function (checkData, i, input) {
                    d.checked = input[i].checked;

                    if (input[i].checked) {
                        $("#line_" + i).show();
                    } else {
                        $("#line_" + i).hide();
                    }
                    update(dataNest);
                    // svg.call(zoom)
                });

            divCategory.append("span")
                .text(d.key);
        }
        legendSpace = width / dataNest.length; // spacing for the legend

        update(dataNest);
    }

    /*
       based on stacked/group bar by bl.ocks.org/mbostock/3943967
    */
    this.drawChartMultiBar = function(data) {
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
};

