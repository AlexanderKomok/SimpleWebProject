//import { url } from "inspector";

//Add data with Ajax 

$('#TestButtonForAjax').on('click', function () {
    var url = $("#TestUrlForAjax").val();
    var title;
    $.ajax({
        url: 'https://noembed.com/embed',
        type: 'get',
        async: false,
        data: { format: 'json', url: url },
        success: function (d) {
            console.log(d);
            var data = JSON.parse(d);
            title = data.title;

        }
    });

    $.ajax({
        type: "POST",
        url: "Player/CreateFromPlayer",
        data: { url: url, title: title },
        success: function () {
            console.log(title);
            var html = "<div class='item'>" + url + "</div>";
            $("#list-to-play").prepend(html);
        }
    })
})


// regex
var re = /^(https?:\/\/)?((www\.)?(youtube(-nocookie)?|youtube.googleapis)\.com.*(v\/|v=|vi=|vi\/|e\/|embed\/|user\/.*\/u\/\d+\/)|youtu\.be\/)([_0-9a-z-]+)/i;
var getVideoId = function (url) { return url.match(re)[7]; }
console.log(getVideoId)



//load Youtube API asynchronously
var tag = document.createElement('script')
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0]
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag)

var player // variable to hold new YT.Player() instance

//initialize a Youtube player
function onYouTubeIframeAPIReady(VideoId) {
    player = new YT.Player('player', {
        height: '315',
        width: '560',
        playerVars: { autoplay: 1 },
        videoId: getVideoId(VideoId),
        events: {
            'onReady': function (event) {
            },
            'onStateChange': onStateChange()
        }
    });
}

function onStateChange() {
    var state = player.getPlayerState()
    if (state == 0) {
        $.ajax({
            type: "POST",
            url: "Player/ChangeFromPlayer",
            data: { url: url},
            contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            async: false

        })
        $.ajax({
            type: "GET",
            url: "Player/DisplayPartialView",   
            contentType: 'application/html; charset=utf-8',
            dataType: 'html',
            async: false,
            success: function (data) {
                $('OutputTable').html(data);
            }
        })

        player.loadVideoById(id);
        player.playVideo();




        //    $.ajax({
        //        type: "GET",
        //        url: "Player/GetListToPlay",
        //        contentType: 'application/html; charset=utf-8',
        //        type: 'GET',
        //        //allow="autoplay",
        //        dataType: 'html',
        //        async: false,
        //        success: function (response) {
        //            var json_obj = $.parseJSON(response);
        //            $.each(json_obj[0], function (index, value) {
        //                if (index == trackUrl) {
        //                    FirstPartUrl = value.trackUrl;
        //                    console.log(FirstPartUrl);
        //                    var ver = "?version=3";
        //                    var url = FirstPartUrl + ver;
        //                    var re = /^(https?:\/\/)?((www\.)?(youtube(-nocookie)?|youtube.googleapis)\.com.*(v\/|v=|vi=|vi\/|e\/|embed\/|user\/.*\/u\/\d+\/)|youtu\.be\/)([_0-9a-z-]+)/i;
        //                    id = url.match(re)[7];
        //                    player.loadVideoById(id);
        //                    player.playVideo();
        //                }
        //            });
        //        }
        //    })
        //});
    }
}
