var songUrl;
//var params = new URLSearchParams(location.search);
//var AlbumId = params.get('AlbumId');
var AlbumId;

$(document).ready(function () {
    PlayOnClick();
    let NextUrl = $("#track0").val();
    songUrl = NextUrl;
    var params = new URLSearchParams(location.search);
    AlbumId = params.get('AlbumId');

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
                        url: "/Album/ChangePlayerStateFromAlbum",
                        data: { songUrl: songUrl, AlbumId: AlbumId},
                        async: false,
                        success: function () {
                            $.ajax({
                                type: "POST",
                                url: "/Album/DisplayPlayAlbumPartial",
                                data: {AlbumId: AlbumId },
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
}

function PlayOnClick() {
    $('.item').off('click');
    $('.item').on('click', function () {
        var songUrl = $(this).find('input').attr("value");
        $.ajax({
            type: "POST",
            url: "/Album/ChangePlayerStateFromAlbum",
            data: { songUrl: songUrl, AlbumId: AlbumId },
            async: false,
            success: function () {
                $.ajax({
                    type: "POST",
                    url: "/Album/DisplayPlayAlbumPartial",
                    data: { AlbumId: AlbumId },
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


function GetId() {
    var nowUrl = $("#PlayItem").val();
    var re = /^(https?:\/\/)?((www\.)?(music\.)?(youtube(-nocookie)?|youtube.googleapis)\.com.*(v\/|v=|vi=|vi\/|e\/|embed\/|user\/.*\/u\/\d+\/)|youtu\.be\/)([_0-9a-z-]+)/i;
    var songId = nowUrl.match(re)[8];
    return songId;
}