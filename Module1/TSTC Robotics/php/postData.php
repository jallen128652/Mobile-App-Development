<?php

$name = $_GET['name'];
$address = $_GET['address'];
$city = $_GET['city'];
$state = $_GET['state'];
$zip = $_GET['zip'];
$phone = $_GET['phone'];
$email = $_GET['email'];
$tech = $_GET['tech'];

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

        <?php echo $zip ?>
        <br>

        <?php echo $phone ?>
        <br>

        <?php echo $email ?>
        <br>

        <?php echo $tech ?>
        
    </body>
     

</html>