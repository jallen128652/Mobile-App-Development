<?php

$name = $_GET['name'];
$address = $_GET['address'];
$city = $_GET['city'];
$state = $_GET['state'];
$comments = $_GET['comments'];

?> 

<html> 

    <head>
        <title>PHP Test Page</title>
    </head> 
    
    <body>
        
        <?php echo $name ?>
        <br>

        <?php echo $address ?>
        <br>

        <?php echo $city ?>
        <br>

        <?php echo $state ?>
        <br>

        <?php echo $comments ?>
        
    </body>
     

</html>