//javascript

//api to fetch historical data of bitcoin Price index


//Parse data into key-value pairs
//@param{object} data object containing historical data of BPI
//function parseData(data) {
//    var arr = [];
//    for (var i in data.bpi) {
//        arr.push({
//            date: new Date(i),
//            value: +data.bpi[i]
//        });
//    }
//    return arr;
//}

function parseDataSF(data) {
    var arr = [];
    for (var i in data) {
        arr.push({
            year: data[i].YValue , //YValue = Year
            value: (Math.log(+data[i].XValue)), //XValue = Amount 
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

function drawChartMultiLine(data)
{
   
    var margin = {top: 30, right: 20, bottom: 70, left: 30},
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
        d.value =+d.value;
    });

    // Define the line
    var gline = d3.line()
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
        dataNest.forEach(function (d, i) {  //key = category
            g.append("path")
                .attr("class", "line")
                .style("stroke", function () { // Add the colours dynamically
                    return d.color = color(d.key);
                })
                .attr("d", gline(d.values));

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


