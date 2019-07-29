var songId;
var songUrl;
$(document).ready(function () {
    //Add data to database and output it to list
    $('#TestButtonForAjax').on('click', function () {
        var url = $("#TestUrlForAjax").val();
        var title;
        $.ajax({
            url: 'https://noembed.com/embed',
            type: 'get',
            async: false,
            data: { format: 'json', url: url },
            success: function (d) {
                var data = JSON.parse(d);
                title = data.title;
                console.log(data);
                console.log(title);
                $.ajax({
                    type: "POST",
                    url: "Player/CreateFromPlayer",
                    data: { url: url, title: title },
                    success: function () {
                        //console.log(title);
                        var html = "<div class='item'>" + title + "</div>";
                        $("#list-to-play").prepend(html);
                    }
                })
            }
        });

        //$.ajax({
        //    type: "POST",
        //    url: "Player/CreateFromPlayer",
        //    data: { url: url, title: title },
        //    success: function () {
        //        //console.log(title);
        //        var html = "<div class='item'>" + url + "</div>";
        //        $("#list-to-play").prepend(html);
        //    }
        //})
    })

    let nowUrl = $("#PlayItem").val();
    var re = /^(https?:\/\/)?((www\.)?(youtube(-nocookie)?|youtube.googleapis)\.com.*(v\/|v=|vi=|vi\/|e\/|embed\/|user\/.*\/u\/\d+\/)|youtu\.be\/)([_0-9a-z-]+)/i;
    let id = nowUrl.match(re)[7];
    let NextUrl = $("#track0").val();


    songId = id;
    songUrl = NextUrl;

});

// load Youtube API code asynchronously
var tag = document.createElement('script')

tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0]
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag)

var player // variable to hold new YT.Player() instance

// define onYouTubeIframeAPIReady() function and initialize a Youtube video player when the API has loaded

function onYouTubeIframeAPIReady() {
    player = new YT.Player('player', {
        height: '390',
        width: '640',
        playerVars: { autoplay: 1, loop: 1 },
        videoId: songId,
        events: {
            'onReady': function (event) { },
            'onStateChange': function (event) {
                var state = player.getPlayerState()
                if (state == 0) {                
                    $.ajax({
                        type: "POST",
                        url: "Player/ChangeFromPlayer",
                        data: { songUrl: songUrl },
                        async: false,
                        success: function () {
                            $.ajax({
                                type: "GET",
                                url: "Player/DisplayPartialView",
                                contentType: 'application/html; charset=utf-8',
                                dataType: 'html',
                                async: false,
                                success: function (data) {
                                    $('OutputTableFromView').html(data);
                                    player.loadVideoById(songId);
                                    player.playVideo();
                                }
                            })
                        },
                    })
                }
            }
        }
    })
}



    //        $.ajax({
    //            type: "POST",                
    //            url: "Player/ChangeFromPlayer",
    //            data: { url: url },
    //            contentType: 'application/html; charset=utf-8',
    //            dataType: 'html',
    //            async: false

    //        })
    //        $.ajax({
    //            type: "GET",
    //            url: "Player/DisplayPartialView",
    //            contentType: 'application/html; charset=utf-8',
    //            dataType: 'html',
    //            async: false,
    //            success: function (data) {
    //                $('OutputTable').html(data);
    //            }
    //        })

    //        player.loadVideoById(id);
    //        player.playVideo();




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
            //                    var getVideoId = function (songUrl) { return songUrl.match(re)[7]; }
            //                    id = url.match(re)[7];
            //                    player.loadVideoById(id);
            //                    player.playVideo();
            //                }
            //            });
            //        }
            //    })
            //});
    //    }
    //}

