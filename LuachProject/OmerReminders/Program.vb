Imports JewishCalendar

Module Program
    ''' <summary>
    ''' Holds the list of locations
    ''' </summary>
    ''' <remarks></remarks>
    Friend LocationsList As List(Of Location)

    Sub New()
        LoadLocations()
    End Sub

    ''' <summary>
    ''' Loads the locations from the settings.
    ''' Note: the xml format for each locations is:
    '''     <L N="Ofakim" H="אופקים" I="Y">    <!--N = name of location, H = hebrew name (optional), I = is the location in Israel? [Y = yes] (optional)-->
    '''         <T>2</T>    <!--Time zone: hour offset from UTC (AKA GMT) -->
    '''         <E>170</E>    <!--Elevation in meters (optional)-->
    '''         <LT>31.32</LT>    <!--Latitude-->
    '''         <LN>-34.62</LN>   <!--Longitude-->
    '''         <CL>30</CL>    <!--Candle-lighting: minutes before sunset (optional)-->
    '''         <TZN>Israel Standard Time</TZN>    <!--Time zone name (optional)-->
    '''     </L>
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub LoadLocations()
        LocationsList = New List(Of Location)()
        Using ms = New System.IO.StringReader(My.Resources.LocationsList)
            Dim settings = New System.Xml.XmlReaderSettings() With {.IgnoreWhitespace = True}
            Using xr = System.Xml.XmlReader.Create(ms, settings)
                While xr.ReadToFollowing("L")
                    Dim name As String = xr.GetAttribute("N").Trim()
                    Dim heb As String = If(xr.GetAttribute("H"), name)
                    Dim inIsrael As Boolean = xr.GetAttribute("I") = "Y"
                    Dim timeZone As Integer
                    Dim elevation As Integer = 0
                    Dim latitude As Double
                    Dim longitute As Double
                    Dim candleLighting As Integer
                    Dim timeZoneName As String = Nothing

                    xr.ReadToDescendant("T")
                    timeZone = xr.ReadElementContentAsInt("T", "")
                    If xr.Name = "E" Then
                        elevation = xr.ReadElementContentAsInt("E", "")
                    End If

                    latitude = xr.ReadElementContentAsDouble("LT", "")
                    longitute = xr.ReadElementContentAsDouble("LN", "")

                    If xr.Name = "CL" Then
                        candleLighting = xr.ReadElementContentAsInt("CL", "")
                    Else
                        candleLighting = If(inIsrael, 30, 18)
                    End If

                    If xr.Name = "TZN" Then
                        timeZoneName = xr.ReadElementContentAsString("TZN", "")
                    ElseIf inIsrael Then
                        timeZoneName = "Israel Standard Time"
                    End If

                    LocationsList.Add(New Location(name, timeZone, latitude, longitute) With {
                         .NameHebrew = heb,
                         .Elevation = elevation,
                         .IsInIsrael = inIsrael,
                         .TimeZoneName = timeZoneName,
                         .CandleLighting = candleLighting
                    })
                End While
                xr.Close()
            End Using
            ms.Close()
        End Using
    End Sub
End Module