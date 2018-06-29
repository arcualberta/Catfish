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
                .attr("stroke-dashoffset", totalLength).transition().duration(100).ease(d3.easeCubicOut)
                .attr("stroke-dashoffset", 0);
            
            // Add the Legend  
            // drawlegend(d);
            
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


    // Loop through each category / key
    //var paths = {};
    //dataNest.forEach(function (d, i) {  //key = category
    //  var path =  g.append("path")
    //        .attr("class", "line")
    //        .style("stroke", function () { // Add the colours dynamically
    //            return d.color;
    //        })
    //    .attr("d", lineFunction(d.values))
    //    .attr("id", "line_" + d.id);
       
    //  var totalLength = path.node().getTotalLength();
    //  path.attr("stroke-dasharray", totalLength + " " + totalLength)
    //      .attr("stroke-dashoffset", totalLength).transition().duration(3000).ease(d3.easeCubicOut)
    //      .attr("stroke-dashoffset", 0);
    // // paths[i] = path;

    //});

  
 
    
    
    update(dataNest);
}

function drawChartMultiBar(data) {
    // Source: https://bl.ocks.org/mbostock/3886208
    //var parseTime = d3.timeParse("%Y");
    //data.forEach(function (d) {
    //    d.year = parseTime(d.year);
    //    d.value = +d.value;
    //});

    //// Nest the entries by category
    //var dataNest = d3.nest()
    //    .key(function (d) { return d.category; })
    //    .entries(data);

    //var m =data.length, // number of series
    //n = dataNest.length; //number of samples  -- category

    var n = 10;// // number of samples
    m = 5;// // number of series
    var data = d3.range(m).map(function () {
        return d3.range(n).map(Math.random);
    });

   

    var margin = { top: 20, right: 30, bottom: 30, left: 40 },
        width = 960 - margin.left - margin.right,
        height = 500 - margin.top - margin.bottom;

    var y = d3.scaleLinear()
        .domain([0, n])
        .rangeRound([height, 0])
            .nice();

    var x = d3.scaleBand()
        .rangeRound([0, width])
        .paddingInner(0.05)
        .align(0.1)
        .domain(d3.range(n));

    var color = d3.scaleOrdinal(d3.schemeCategory10);

    var svg = d3.select("svg")
        .attr("width", width + margin.left + margin.right)
        .attr("height", height + margin.top + margin.bottom)
      .append("g")
        .attr("transform", "translate(" + margin.left + "," + margin.top + ")");

    svg.append("g").selectAll("g")
            .data(d3.stack().keys(d3.range(n))(data))
      .enter().append("g")
        .style("fill", function (d, i) {
            return color(d.key);
        })
        .selectAll("rect")
      .data(function (d) {
          return d
      })
        .enter().append("rect")
          .attr("x", function (d, i) {
              return x(i);
             // return x(d.data.key);
          })
         //.attr("y", function (d, i) {
         //    return y(d);
         //   //return  function(v, j){ return v[j].value;}
         //})
         // .attr("height", function (d, i) {
         //      return y(d[0]) - y(d[1]);
         //     //return function (v, j) {
         //     //    y([v[j].value]) - y([v[j + 1].value]);
         //     //}
         // })

            //example
          .attr("y", function (d) {
              return y(d[1]);
          })
          .attr("height", function (d) {
              return y(d[0]) - y(d[1]);
          })
          .attr("width", x.bandwidth());

}




