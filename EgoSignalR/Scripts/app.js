"use strict";

$(function () {
    var egoHub = $.connection.egoHub;



    egoHub.client.broadcastMessage = function (data) {
        console.log(data);
    };

    egoHub.client.updateBusInfo = function (data) {
        console.log(data);
    };


    //$.connection.hub.logging = true;
    $.connection.hub.start().done(function () {
        //$('#sendmessage').click(function () {
        //    // Call the Send method on the hub. 
        //    chat.server.send($('#displayname').val(), $('#message').val());
        //    // Clear text box and reset focus for next comment. 
        //    $('#message').val('').focus();
        var stopNo = 10986;
        egoHub.server.getBusInfo(stopNo);

    });



});