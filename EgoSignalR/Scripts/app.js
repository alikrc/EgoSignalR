"use strict";

$(function () {
    var egoHub = $.connection.egoHub;



    egoHub.client.broadcastMessage = function (data) {
        console.log(data);
    };

    egoHub.client.updateBusInfo = function (data) {
        console.log(data);
        $("#dvResult").html(data);
    };

    egoHub.client.updateData = function (data) {
        console.log(data);
        $("#dvResult").text(data);
    };
    $.connection.hub.logging = true;
    $.connection.hub.start();



    $('#btnSubmit').click(function () {

        var lineNumber = $("#txtLineNo").val();
        var stopNo = $("#txtStopNo").val();

        //parse for int
        lineNumber = parseInt(lineNumber);
        stopNo = parseInt(stopNo);
        console.log(lineNumber + " " + stopNo);

        egoHub.server.startPoint(lineNumber, stopNo);
    });


    ////
    //$.connection.hub.start().done(function () {
    //    //$('#sendmessage').click(function () {
    //    //    // Call the Send method on the hub. 
    //    //    chat.server.send($('#displayname').val(), $('#message').val());
    //    //    // Clear text box and reset focus for next comment. 
    //    //    $('#message').val('').focus();



    //    //egoHub.server.getBusInfo(stopNo);

    //    var lineNumber = 541;
    //    var stopNo = 10986;
    //    egoHub.server.startPoint(lineNumber, stopNo);
    //});



});

function loadDefaultValues() {

};