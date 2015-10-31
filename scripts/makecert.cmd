makecert.exe ^
-n "CN=FulcrumCARoot" ^
-r ^
-pe ^
-a sha512 ^
-len 4096 ^
-cy authority ^
-sv Fulcrum.pvk ^
IdentitySrvCAROOT.cer
 
pvk2pfx.exe ^
-pvk Fulcrum.pvk ^
-spc IdentitySrvCAROOT.cer ^
-pfx Fulcrum.pfx ^
-po password123