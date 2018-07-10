//javascript

//Parse data into key-value pairs

function parseDataSF(data) {
    var arr = [];
    for (var i in data) {
        arr.push({
            year: data[i].YValue/XScale , //YValue = Year
            value: +(data[i].XValue / YScale),//(Math.log(+data[i].XValue)), //XValue = Amount 
            category: data[i].Category
        });
    }
   
    return arr;
}
/*
function drawChartLine(data) {
    var svgWidth = 600, svgHeight = 400;
    var margin = { top: 20, right: 20, bottom: 30, left: 50 };
    var width = svgWidth - margin.left - margin.right;
    var height = svgHeight - margin.top - margin.bottom;

    var svg = d3.select("svg").attr("width", svgWidth).attr("height", svgHeight);

    var g = svg.append('g').attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    var x = d3.scaleLinear().rangeRound([0, width]);// d3.scaleTime().rangeRound([0, width]);
    var y = d3.scaleLinear().rangeRound([height, 0]);

    var line = d3.line().x(function (d) { return x(d.year); })
                         .y(function (d) { return y(d.value); })
    x.domain(d3.extent(data, function (d) { return d.year }));
    y.domain(d3.extent(data, function (d) { return d.value }));
    g.append("g").attr("transform", "translate(0," + height + ")")
	             .call(d3.axisBottom(x))
                  .append("text").attr("fill", "#0000ff").attr("y", 30).attr("x", svgWidth/2).attr("text-anchor", "end").text(XLabel);
    //.select(".domain").remove();

    g.append("g").call(d3.axisLeft(y)).append("text").attr("fill", "#0000ff")
	              .attr("transform", "rotate(-90)")
				  .attr("y", -75)
                 // .attr("x", 5)
				  .attr("dy", "0.17em")
				  .attr("text-anchor", "end")
				  .text(YLabel);

    g.append("path").datum(data).attr("fill", "none")
	                            .attr("stroke", "steelblue")
								.attr("stroke-linejoin", "round")
								.attr("stroke-linecap", "round")
								.attr("stroke-width", 1.5)
								.attr("d", line);
}

function drawChartBar(dataset)
{
    var svgWidth = 900, svgHeight = 500, barPadding = 5;
    var barWidth = (svgWidth / dataset.length);
    var margin = {top: 20, right:20, left:40, bottom:30};
    var width = svgWidth - margin.left - margin.right;
    var height = svgHeight - margin.top - margin.bottom;
  
    var svg = d3.select("svg").attr("width", svgWidth).attr("height", svgHeight);

    var x = d3.scaleBand()
              .rangeRound([0, width]).padding(0.1);
    var y = d3.scaleLinear().rangeRound([height, 0]);
   

    var g =  svg.append("g")
         .attr("transform", "translate(" + margin.left + "," + margin.top + ")");
 
    x.domain(dataset.map(function (d) { return d.year; }));
    y.domain([0, d3.max(dataset, function (d) {
        return d.value;
    })]);

    g.append("g")
     .attr("transform", "translate(0," + height + ")").call(d3.axisBottom(x))
       .append("text").attr("fill", "#0000ff").attr("y", 30).attr("x", svgWidth / 2).attr("text-anchor", "end").text(XLabel);

    g.append("g").attr("class", "axis")
                  .call(d3.axisLeft(y))//.ticks(20, "M"))
                  .append("text").attr("fill", "#0000ff")
                  .attr("transform", "rotate(-90)")
                  .attr("y", -35)
                  .attr("dy", "0.71em")
                  .attr("text-anchor", "end")
                  .text(YLabel);

    g.selectAll(".bar")
                  .data(dataset)
				  .enter()
				  .append("rect")
                  .attr("class", "bar")
                  .attr("x", function (d) { return x(d.year); })
				  .attr("y", function (d) {
				    
				      return y(d.value);
				  })
				  .attr("height", function (d) { return height - y(d.value); })
				  .attr("width", x.bandwidth());
}
*/
function selectSvg()
{
    var margin = { top: 30, right: 20, bottom: 70, left: 70 },
        width = 600 - margin.left - margin.right,
        height = 400 - margin.top - margin.bottom;

    // Set the ranges
    var x = d3.scaleTime().range([0, width]);
    var y = d3.scaleLinear().range([height, 0]);

    // Adds the svg canvas
    var svg = d3.select("svg").attr("width", 600).attr("height", 400);
    return svg;
}

function drawChartMultiLine(data) {

    var margin = { top: 30, right: 20, bottom: 70, left: 70 },
        width = 600 - margin.left - margin.right,
        height = 400 - margin.top - margin.bottom;

   

    // Set the ranges
    var x = d3.scaleTime().range([0, width]);
    var y = d3.scaleLinear().range([height, 0]);

    // Adds the svg canvas
    var svg = d3.select("svg.line-chart").attr("width", 600).attr("height", 400);

    var g1 = svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + " )");


    var divLegend = d3.select("div.legend");

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
        dataNest.forEach(function (d) {
            if (!d.checked) return;

            var path = g1.append("path")
                        .attr("class", "line")
                        .style("stroke", function () { // Add the colours dynamically
                                                return d.color;
                                                    })
                        .attr("d", lineFunction(d.values))
                        .attr("id", "line_" + d.id);

            var totalLength = path.node().getTotalLength();
            path.attr("stroke-dasharray", totalLength + " " + totalLength)
                .attr("stroke-dashoffset", totalLength).transition().duration(0).ease(d3.easeCubicOut)
                .attr("stroke-dashoffset", 0);

            path.exit().remove();
        });
    }

    function drawAxis(g1) {
        // Add the X Axis
        g1.append("g")
            .attr("class", "axis")
            .attr("transform", "translate(0," + height + ")")
            .call(d3.axisBottom(x))
             .append("text").attr("fill", "#1f77b4").attr("y", 35).attr("x", width / 2).attr("text-anchor", "end").text(XLabel);

        // Add the Y Axis
        g1.append("g")
            .attr("class", "axis")
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
            if(dt.checked){
                var check = d3.max(dt.values, function (d) {
                    return d.value;
                });

                if (check > new_max) { new_max = check; }
            }
        });
       
        y.domain([0, new_max]);
        d3.select("g1").remove();

        drawAxis(g1);
        redrawChart(dataNest, g1);
    }

    dataNest.forEach(function (d, i) {
        d.checked = true;
        d.color = color(d.key);
        d.id = d.key;

        //Add the legend
        var divCategory = divLegend.append("div")
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
                    $("#line_" + d.id).show();
                } else {
                    $("#line_" + d.id).hide();
                }
                update(dataNest);

            });

        divCategory.append("span")
            .text(d.key);
    });

    function drawLegend(d)
    {
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
                    $("#line_" + d.id).show();
                } else {
                    $("#line_" + d.id).hide();
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
function drawChartMultiBar(data) {
    var svgWidth = 1000, svgHeight = 500;
    var cats = [];
    var years = [];
    var nested = d3.nest()
          .key(function (d) { return d["category"]; })
          .entries(data)
    var i = 0;
    nested.forEach(function (d) {
        if($.inArray(d.key, cats) < 0)
        {
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
    m =  years.length, //number of samples per layer
 
    stack = d3.stack().keys(cats);
    stack.value(function (d, key) {
        var v=0;
        d.values.forEach(function (l) { 
            if (l.category === key)
                v = l.value;
          
        })
       return v;
    });
   
   
   var myData = d3.nest().key(function (d) { return d.year}).entries(data);//group data by x (i.e year)
 
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
        .domain(years.map(function(d){return d;}))//display year
        .rangeRound([0, width]).paddingInner(0.05).align(0.01);
       

    var y = d3.scaleLinear()
        .domain([0, yStackMax])
        .range([height, 0]);

    var color = d3.scaleOrdinal(d3.schemeCategory10);

    var xAxis = d3.axisBottom(x)
        .tickSize(0)
        .tickPadding(6);
    var yAxis = d3.axisLeft(y).tickPadding(6);


    var svg = d3.select("body").append("svg")
        .attr("width", svgWidth)
        .attr("height", height + margin.top + margin.bottom);
    var g1 = svg.append("g")
        .attr("transform", "translate(" + (margin.left +50 )+ "," + margin.top + ")");

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
        .on("mouseover", function(){tooltip.style("opacity", 1)})
         .on("mouseout", function () { tooltip.style("opacity", 0) })
         .on("mousemove", function (d) {
              var xPos = d3.mouse(this)[0] - 5;
              var yPos = d3.mouse(this)[1] - 5;
              var amt = d[1] - d[0]
              tooltip.attr("transform", "translate(" + xPos + "," + yPos + ")");
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
    tooltip.append("rect").attr("width", 80)
                          .attr("height", 20)
                          .attr("fill", "white")
                          .style("opacity", 0)
                          .style("border", "solid 1px Red");
    tooltip.append("text").attr("x", 30)
                          .attr("dy", "1.2em")
                          .style("text-anchor", "middle")
                          .attr("font-size", "12px")
                          .attr("font-weight", "bold");
    
    d3.selectAll("input").on("change", change);

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
                return x(d.data.key); })
            .attr("width", x.bandwidth() - 1);

    } 

}


