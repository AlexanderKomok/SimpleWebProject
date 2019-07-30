var songId;
var songUrl;
$(document).ready(function () {

    AddNewUrl();

    PlayOnClick();




    let NextUrl = $("#track0").val();
    songUrl = NextUrl;
});



// load Youtube API code asynchronously
var tag = document.createElement('script')
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0]
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag)

var player // variable to hold new YT.Player() instance

// define onYouTubeIframeAPIReady() function and initialize a Youtube video player when the API has loaded
setTimeout(function () { }, 500);
function onYouTubeIframeAPIReady() {
    player = new YT.Player('player', {
        height: '390',
        width: '640',
        playerVars: { autoplay: 1, loop: 1 },
        videoId: GetId(),
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
                                    $('.body-content').html(data);
                                    setTimeout(function () { }, 500);
                                    onYouTubeIframeAPIReady();
                                }

                            })
                        },
                    })

                }
            }
        }
    })
    function GetId() {
        var nowUrl = $("#PlayItem").val();
        var re = /^(https?:\/\/)?((www\.)?(youtube(-nocookie)?|youtube.googleapis)\.com.*(v\/|v=|vi=|vi\/|e\/|embed\/|user\/.*\/u\/\d+\/)|youtu\.be\/)([_0-9a-z-]+)/i;
        var songId = nowUrl.match(re)[7];
        return songId;
    }
}

function AddNewUrl() {
    //$('#AddButtonForAjax').unbind('click');
    $('#AddButtonForAjax').on('click', function () {
        var url = $("#AddUrlForAjax").val();
        var title;
        $.ajax({
            url: 'https://noembed.com/embed',
            type: 'get',
            async: false,
            data: { format: 'json', url: url },
            success: function (d) {
                var data = JSON.parse(d);
                title = data.title;
                $.ajax({
                    type: "POST",
                    url: "Player/CreateFromPlayer",
                    data: { url: url, title: title },
                    success: function () {
                        //In future it will work
                        //var html = "<div class='item'>" + title + "</div> <input type='hidden' value=" />";
                        //$("#list-to-play").prepend(html);

                        //Its temporary
                        $.ajax({
                            type: "GET",
                            url: "Player/DisplayPartialView",
                            contentType: 'application/html; charset=utf-8',
                            dataType: 'html',
                            async: false,
                            success: function (data) {
                                $('.body-content').html(data);
                                setTimeout(function () { }, 500);
                                onYouTubeIframeAPIReady();
                            }

                        })
                    }
                })
            }
        });
    })

}


function PlayOnClick() {
    $('.item').off('click');
    //$('.item').unbind('click');
    $('.item').on('click', function () {
        var songUrl = $(this).find('input').attr("value");
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
                        $('.body-content').html(data);
                        setTimeout(function () { }, 500);
                        onYouTubeIframeAPIReady();
                        PlayOnClick();
                    }

                })
            },

        })
    })
}