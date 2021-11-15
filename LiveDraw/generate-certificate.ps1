$cert = New-SelfSignedCertificate -CertStoreLocation cert:\localmachine\my -dnsname localhost
$pwd = ConvertTo-SecureString -String ‘livedraw’ -Force -AsPlainText
$path = ‘cert:\localMachine\my\’ + $cert.thumbprint

Export-PfxCertificate -cert $path -FilePath livedraw.pfx -Password $pwd