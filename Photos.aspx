<%@ Page Language="C#" Inherits="flickr.mendhak.com.Photos" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

    <script type="text/javascript" src="jquery-1.4.2.min.js"></script>

    <style type="text/css">
        div#imageContainer
        {
            width: 1024px;
            margin-left: auto;
            margin-right: auto; 
            display: block;
        }
        img#theImage
        {
            margin-left: auto;
            margin-right: auto;
            display: block;
        }
        div#navLeft
        {
            position: absolute;
            width: 110px;
            height: 600px;
            z-index: 20;
            display: table-cell;
            vertical-align: middle;
            text-align: center;
            cursor: pointer;
            background-image: url('spacer.gif');
            background-repeat: repeat;
        }
        div#navRight
        {
            position: absolute;
            width: 110px;
            height: 600px;
            z-index: 20;
            display: table-cell;
            vertical-align: middle;
            text-align: center;
            cursor: pointer;
            
            background-image: url('spacer.gif');
            background-repeat: repeat;
        }
        img#next, img#prev
        {
            margin-top: 300px;
            vertical-align: middle;
            display:none;
        }
        #ajaxLoad
        {
            display: none;
            position: absolute;
            z-index: 30;
        }
        img.lessOpacity
        {
            opacity: 0.1;
            filter: alpha(opacity=10);
            zoom: 1; /* needed to trigger "hasLayout" in IE if no width or height is set */
        }
    </style>

    <script type="text/javascript">

		var latestImageIndex = 1;
		
	
		function getUrl(urlType, size, index)
		{
			return '/f.ashx?nsid=69135870@N00&get=' + urlType +'&size=' + size + '&caching=true&latest=' + index;
		}
		

		function replaceImage(imgUrl)
		{
		   $(this).data('locked', true);
		   
        	
            //$("#theImage").attr('class', 'lessOpacity');

            $("#theImage").fadeOut('fast', function() {             
            	$("#ajaxLoad").fadeIn('fast');
             	$("#theImage").attr('src', imgUrl); });

		}


        function goNext() {

            if ($(this).data('locked') == true) {
              
                return;
            }
            
            latestImageIndex++;

           replaceImage(nextImage);
        }


        function goPrevious() {

            if ($(this).data('locked') == true) {
             
                return;
            }
            
            if(latestImageIndex >= 2)
            {
                latestImageIndex--;
            }
            else
            {
            	return;
            }

            replaceImage(getUrl('img','large', latestImageIndex));
        }

		function cacheNextImage()
		{
		
				$.get(getUrl('imgurl','large',latestImageIndex+1), function(data){
				    pic1= new Image(100,25); 
					pic1.src=data; 
					nextImage = data;
				 });
				 
				
				 
			
		}


        function imageReady() {


			cacheNextImage();

            $("#ajaxLoad").fadeOut('fast');
            //$("#theImage").attr('class', '');
            $("#theImage").fadeIn('medium');
          
            $(this).data('locked', false);
          
            
            var imageHeight = $("#theImage").attr('naturalHeight');
			var viewPortHeight = $(window).height()
			
			if(viewPortHeight > imageHeight)
			{
				$("#theImage").attr('height', imageHeight);
			}
			else
			{
				$("#theImage").attr('height', viewPortHeight-20);
			}
                
            var leftDivPos = 0;
            var rightDivPos = 1024;

            leftDivPos = $("#theImage").offset().left;
            rightDivPos = $("#theImage").offset().left + $("#theImage").width() - 110;


            //add left nav
            $("#navLeft").click(function() { goPrevious(); });
            $("#navRight").click(function() { goNext(); });

            //$("#ajaxLoad").css
            //$("#imageContainer").append("")
            
            $("#navLeft").css('display', 'table-cell');
            
            $("#navLeft").css('left', leftDivPos + 'px');
            
            $("#navLeft").css('top', $("#imageContainer").offset().top + 'px');
            
            $("#navLeft").css('height', $("#imageContainer").height() + 'px');
            

            //add right nav
            //$("#imageContainer").append("<div id='navRight' title='Next'><img src='next.gif' id='next' style='display:none;' /></div>")
            $("#navRight").css('display', 'table-cell');
            $("#navRight").css('left', rightDivPos + 'px');
            $("#navRight").css('top', $("#imageContainer").offset().top + 'px');
            $("#navRight").css('height', $("#imageContainer").height() + 'px');

            //fade in fade out
            $("#navRight").mouseenter(function() {  $("#next").fadeIn(100); });
            $("#navRight").mouseleave(function() { $("#next").fadeOut(100); });
            $("#navLeft").mouseenter(function() { $("#prev").fadeIn(100); });
            $("#navLeft").mouseleave(function() {  $("#prev").fadeOut(100); });

            $("#navLeft").click(function() { goPrevious(); });
            $("#navRight").click(function() { goNext(); });

            //$("#ajaxLoad").css('left', $("#theImage").offset().left + $("#theImage").width() / 2 + 'px');
            //$("#ajaxLoad").css('top', $("#theImage").offset().top + $("#theImage").height() / 2 + 'px');

        }


		var nextImage;
		var prevImage;


        $(document).ready(function() {
        
         	$("#ajaxLoad").css('left', $(window).width() / 2 + 'px');
            $("#ajaxLoad").css('top', $(window).height() / 2 + 'px');
			//$("#ajaxLoad").fadeIn('fast');
        
            $("#theImage").load(function() {
                imageReady();
            }
            );
		
        });

    </script>

</head>
<body style="background-color: Black;">
    <form id="form1" runat="server">
    <img id="ajaxLoad" src="ajaxload.gif" alt="Please wait..." />
    <div id="imageContainer">
    <div id="navLeft" title="Previous"><img src="Previous.gif" id="prev" /></div>
        <img id="theImage" height="600px"  src="/f.ashx?nsid=69135870@N00&get=img&latest=1&size=large" />
    </div>
    <div id="navRight" title="Next"><img src="Next.gif" id="next" /></div>
    </form>
</body>
</html>
