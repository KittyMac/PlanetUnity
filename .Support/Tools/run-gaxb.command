#!/bin/sh

newPath=`echo $0 | awk '{split($0, a, ";"); split(a[1], b, "/"); for(x = 2; x < length(b); x++){printf("/%s", b[x]);} print "";}'`
cd "$newPath"

./gaxb csharp ./PlanetUnity.xsd -t ./gaxb.templates -o ../../

#./gaxb csharp ./PlanetUnity.xsd -t ./gaxb.templates -o ./TEST/