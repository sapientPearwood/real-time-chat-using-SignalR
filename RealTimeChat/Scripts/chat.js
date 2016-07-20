"use strict";
$(function () {
    // Reference the auto-generated proxy for the hub.
    var chat = $.connection.chatHub;
    var room = $(".room").attr("id");

    // Create a function that the hub can call back to display messages.
    chat.client.addChatMessage = function (name, message) {
        //Add the message to the page.
        $("#discussion").append("<p><strong>" + htmlEncode(name) + "</strong>: " + htmlEncode(message) + "</p>");
    };

    //adds user to room
    chat.client.joinRoom = function () {
        chat.server.addToRoom(room);
    };

    //show users in room
    chat.client.displayUsers = function (users) {
        var html = "";
        var i;
        for (i = 0; i < users.length; i += 1) {
            if (users[i]) {
                html += "<li id='" + htmlEncode(users[i]) + "' class='user'><p class='user-name'>" + htmlEncode(users[i]) + "</p><section class='menu'><a href='#' class='mute'>mute</a></section></li>";
            }
        }

        $("#online-users").html(html);
        $("#user-count").html("Users Online - " + users.length);

        //set click event to bring up user menu
        addClickToGroup("user", function (element) {
            $(element).children(".menu").toggle();
        });
        //set click event to mute user
        addClickToGroup("menu", function (element) {
            chat.server.muteUser($(element).prev().text());
        });
    };

    //shows room name
    chat.client.displayRoom = function () {
        $("#room-header").html("Room: " + room);
    };

    //Set initial focus to message input box.
    $("#message").focus();

    //Start the connection.
    $.connection.hub.start().done(function () {

        //join chat room
        chat.server.joinGroup(room);
        //get users
        chat.server.getUsers(room);

        $("#send-message").click(function () {
            //Call the Send method on the hub.
            chat.server.send($("#display-name").val(), $("#message").val(), room);
            //Clear text box and reset focus for next comment.
            $("#message").val("").focus();
        });

    });

    //for encoding messages
    function htmlEncode(value) {
        var encodedValue = $("<div />").text(value).html();
        return encodedValue;
    }

    //adds click events, along with a function to a group of elements
    function addClickToGroup(elementClass, clickFunction) {
        $("." + elementClass).each(function () {
            $(this).click(function () {
                clickFunction(this);
            });
        });
    }
});
