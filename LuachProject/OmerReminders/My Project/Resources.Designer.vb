﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.42000
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("OmerReminder.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property help() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("help", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to &lt;?xml version=&quot;1.0&quot; encoding=&quot;utf-8&quot;?&gt;&lt;LS&gt;&lt;L N=&quot;Ofakim&quot; H=&quot;אופקים&quot; I=&quot;Y&quot;&gt;&lt;T&gt;2&lt;/T&gt;&lt;E&gt;170&lt;/E&gt;&lt;LT&gt;31.32&lt;/LT&gt;&lt;LN&gt;-34.62&lt;/LN&gt;&lt;/L&gt;&lt;L N=&quot;Eilat&quot; H=&quot;אילת&quot; I=&quot;Y&quot;&gt;&lt;T&gt;2&lt;/T&gt;&lt;LT&gt;29.55&lt;/LT&gt;&lt;LN&gt;-34.95&lt;/LN&gt;&lt;/L&gt;&lt;L N=&quot;Elad&quot; H=&quot;אלעד&quot; I=&quot;Y&quot;&gt;&lt;T&gt;2&lt;/T&gt;&lt;E&gt;150&lt;/E&gt;&lt;LT&gt;32.05&lt;/LT&gt;&lt;LN&gt;-34.95&lt;/LN&gt;&lt;/L&gt;&lt;L N=&quot;Ashdod&quot; H=&quot;אשדוד&quot; I=&quot;Y&quot;&gt;&lt;T&gt;2&lt;/T&gt;&lt;LT&gt;31.78&lt;/LT&gt;&lt;LN&gt;-34.63&lt;/LN&gt;&lt;/L&gt;&lt;L N=&quot;Ashkelon&quot; H=&quot;אשקלון&quot; I=&quot;Y&quot;&gt;&lt;T&gt;2&lt;/T&gt;&lt;LT&gt;31.65&lt;/LT&gt;&lt;LN&gt;-34.57&lt;/LN&gt;&lt;/L&gt;&lt;L N=&quot;Be&apos;er Ya&apos;akov&quot; H=&quot;באר יעקב&quot; I=&quot;Y&quot;&gt;&lt;T&gt;2&lt;/T&gt;&lt;LT&gt;31.93&lt;/LT&gt;&lt;LN&gt;-34.83&lt;/LN&gt;....
        '''</summary>
        Friend ReadOnly Property LocationsList() As String
            Get
                Return ResourceManager.GetString("LocationsList", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property OutlookLogo() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("OutlookLogo", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized resource of type System.Drawing.Bitmap.
        '''</summary>
        Friend ReadOnly Property WindowsLogo() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("WindowsLogo", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
    End Module
End Namespace
