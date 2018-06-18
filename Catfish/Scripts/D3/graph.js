//javascript

//Parse data into key-value pairs

function parseDataSF(data) {
    var arr = [];
    for (var i in data) {
        arr.push({
            year: data[i].YValue , //YValue = Year
            value: (+data[i].XValue),//(Math.log(+data[i].XValue)), //XValue = Amount 
            category: data[i].Category
        });
    }
    return arr;
}

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
				  .attr("y", -35)
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
    var svg = d3.select("svg").attr("width", 600).attr("height", 400);

    var g = svg.append("g").attr("transform", "translate(" + margin.left + "," + margin.top + " )");

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
    // Scale the range of the data

    x.domain(d3.extent(data, function (d) { return d.year; }));
    y.domain([0, d3.max(data, function (d) { return d.value; })]);

    // Nest the entries by category
    var dataNest = d3.nest()
        .key(function (d) { return d.category; })
        .entries(data);

    // set the colour scale
    var color = d3.scaleOrdinal(d3.schemeCategory10);

    legendSpace = width / dataNest.length; // spacing for the legend


    // Loop through each category / key
    var paths = {};
    dataNest.forEach(function (d, i) {  //key = category
      var path =  g.append("path")
            .attr("class", "line")
            .style("stroke", function () { // Add the colours dynamically
                return d.color = color(d.key);
            })
        .attr("d", lineFunction(d.values));
       
      var totalLength = path.node().getTotalLength();
      path.attr("stroke-dasharray", totalLength + " " + totalLength)
          .attr("stroke-dashoffset", totalLength).transition().duration(3000).ease(d3.easeCubicOut)
          .attr("stroke-dashoffset", 0);
     // paths[i] = path;

        // Add the Legend  
        g.append("text")
            .attr("x", (legendSpace / 2) + i * legendSpace)  // space legend
            .attr("y", height + (margin.bottom / 2) + 5)
            .attr("class", "legend")    // style the legend
            .style("fill", function () { // Add the colours dynamically
                return d.color = color(d.key);
            })
            .text(d.key);

    });

    //paths.forEach(function (p, i) {
    //    d3.select(p).attr("stroke-dasharray", p.getTotalLegth[0]).attr("stroke-dashoffset", p.getTotalLength[0])
    //    .transition().duration(5000).ease("linear").attr("stroke-dashoffset", 0);
    //});
 
    // Add the X Axis
    g.append("g")
        .attr("class", "axis")
        .attr("transform", "translate(0," + height + ")")
        .call(d3.axisBottom(x))
         .append("text").attr("fill", "#0000ff").attr("y", -8).attr("x", width).attr("text-anchor", "end").text(XLabel);

    // Add the Y Axis
    g.append("g")
        .attr("class", "axis")
        .call(d3.axisLeft(y))
        .append("text").attr("fill", "#0000ff")
              .attr("transform", "rotate(-90)")
              .attr("y", 10)
              .attr("dy", "0.17em")
              .attr("text-anchor", "end")
              .text(YLabel);

    
}

function drawChartMultiBar(data) {
    // Source: https://bl.ocks.org/mbostock/3886208
    var parseTime = d3.timeParse("%Y");
    data.forEach(function (d) {
        d.year = parseTime(d.year);
        d.value = +d.value;
    });

    // Nest the entries by category
    var dataNest = d3.nest()
        .key(function (d) { return d.category; })
        .entries(data);

    //var n = 10;// // number of samples
    //m = 5;// // number of series
    //var data = d3.range(m).map(function () {
    //    return d3.range(n).map(Math.random);
    //});
    var n =data.length, // number of samples
    m = dataNest.length; // number of series
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
            .data(d3.stack().keys(d3.range(n))(dataNest))
      .enter().append("g")
        .style("fill", function (d, i) {
            return color(d);
        })
        .selectAll("rect")
      .data(function (d) {
          return d
      })
        .enter().append("rect")
          .attr("x", function (d, i) {
              //return x(i);
              return x(d.data.key);
          })
         .attr("y", function (d, i) {
             //return y(d);
            return  function(v, j){ return v[j].value;}
         })
          .attr("height", function (d, i) {
              // return y(d[0]) - y(d[1]);
              return function (v, j) {
                  y([v[j].value]) - y([v[j + 1].value]);
              }
          })
          //.attr("y", function (d) {
          //    return y(d[1]);
          //})
          //.attr("height", function (d) {
          //    return y(d[0]) - y(d[1]);
          //})
          .attr("width", x.bandwidth());

}




