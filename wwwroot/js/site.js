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

// Output Play 

//    <script type="text/javascript" language="javascript">
//        var UrlPlaySong = ""
//    function Play() {

//        var trackUrl = "trackUrl"
//        $.ajax({
//            type: "GET",
//        url: "Player/GetPlay",
//        contentType: 'application/html; charset=utf-8',
//        type: 'GET',
//        dataType: 'html',
//            success: function (response) {

//                var json_obj = $.parseJSON(response);

//                $.each(json_obj[0], function (index, value) {
//                    //console.log(index + ':' + value);
//                    if (index == trackUrl) {
//            UrlPlaySong = value;

//        $('#playing1').html(value);

//    }
//});
//}

//})
//return UrlPlaySong;
//}
//$(document).ready(Play);
////console.log(UrlPlaySong)
//</script> 

//    Output List to play 

//    <script type="text/javascript" language="javascript">
//        console.log(UrlPlaySong)
//    function ListToPlay() {
//            $.ajax({
//                type: "GET",
//                url: "Player/GetListToPlay",
//                success: function (arr) {
//                    $.map(arr, function (value, index) {
//                        var html = "<div class='item'>" + value.trackUrl + "</div>";
//                        $("#list-to-play").prepend(html);
//                    })
//                }
//            })
//        }

//        $(document).ready(ListToPlay);
//</script> *@

//    @* Output History *@
//    @*
//    <script type="text/javascript" language="javascript">
//        //var trackUrl = "trackUrl"
//        $.ajax({
//            type: "GET",
//        url: "Player/GetHistory",
//        contentType: 'application/html; charset=utf-8',
//        type: 'GET',
//        dataType: 'html',
//        success: function (response) {
//            var json_obj = $.parseJSON(response);
//        console.log(json_obj);
//        var arr = $.makeArray(json_obj)
//        console.log(arr);
//            $.map(arr, function (value, index) {
//                var html = "<div class='item'>" + value.trackUrl + "</div>";
//        $("#history").prepend(html);
//    })
//}
//})
//</script> *@


                            //Get Play

var UrlPlaySong = ""
var currId;

    var trackUrl = "trackUrl"
    $.ajax({
        type: "GET",
        url: "Player/GetPlay",
        contentType: 'application/html; charset=utf-8',
        type: 'GET',
        dataType: 'html',
        async: false,
        success: function (response) {
            console.log(response)
            var json_obj = $.parseJSON(response);
            console.log(json_obj)
            $.each(json_obj[0], function (index, value) {
                //console.log(index + ':' + value);
                if (index == trackUrl) {
                    UrlPlaySong = value;

                }
            })
        }
    });



var url = UrlPlaySong;
let re = /^(https?:\/\/)?((www\.)?(youtube(-nocookie)?|youtube.googleapis)\.com.*(v\/|v=|vi=|vi\/|e\/|embed\/|user\/.*\/u\/\d+\/)|youtu\.be\/)([_0-9a-z-]+)/i;
var PlaySongid = url.match(re)[7];

//Output Player 


//load Youtube API asynchronously
var tag = document.createElement('script')
tag.src = "https://www.youtube.com/iframe_api";
var firstScriptTag = document.getElementsByTagName('script')[0]
firstScriptTag.parentNode.insertBefore(tag, firstScriptTag)

var player // variable to hold new YT.Player() instance

//initialize a Youtube player

function onYouTubeIframeAPIReady() {
    player = new YT.Player('player', {
        height: '315',
        width: '560',
        playerVars: { autoplay: 1 },
        videoId: PlaySongid,
        events: {
            'onReady': function (event) {
            },
            'onStateChange': function (event) {
                var state = player.getPlayerState()
                if (state == 0) {
                    var FirstPartUrl;
                    //Play Next Song
                    $(document).ready(function () {
                        var trackUrl = "trackUrl"
                        //get NextSongToPlay and play that song
                        $.ajax({
                            type: "GET",
                            url: "Player/GetListToPlay",
                            contentType: 'application/html; charset=utf-8',
                            type: 'GET',
                            dataType: 'html',
                            async: false,
                            success: function (response) {
                                var json_obj = $.parseJSON(response);
                                $.each(json_obj[0], function (index, value) {
                                    if (index == trackUrl) {
                                        FirstPartUrl = value;
                                        console.log(FirstPartUrl);
                                        var ver = "?version=3";
                                        var url = FirstPartUrl + ver;
                                        let re = /^(https?:\/\/)?((www\.)?(youtube(-nocookie)?|youtube.googleapis)\.com.*(v\/|v=|vi=|vi\/|e\/|embed\/|user\/.*\/u\/\d+\/)|youtu\.be\/)([_0-9a-z-]+)/i;
                                        id = url.match(re)[7];
                                        player.loadVideoById(id);
                                        player.playVideo();
                                    }
                                });
                            }
                        })
                    });
                    //Change database
                    $(document).ready(function () {
                        var url = FirstPartUrl;
                        console.log(url)
                        $.ajax({
                            type: "POST",
                            url: "Player/ChangeFromPlayer",
                            data: { url: url },
                            success: function () {
                            }
                        })
                    })
                }
            }

        }
    })
}
