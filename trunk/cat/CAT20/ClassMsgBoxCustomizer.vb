Imports System.Text
Imports System.Runtime.InteropServices

Public Class nobugz
    Private Shared mLabels() As String    '' Desired new labels
    Private Shared mLabelIndex As Integer '' Next caption to update

    Public Shared Sub PatchMsgBox(ByVal labels() As String)
        ''--- Updates message box buttons
        mLabels = labels
        Application.OpenForms(0).BeginInvoke(New FindWindowDelegate(AddressOf FindMsgBox), GetCurrentThreadId())
    End Sub

    Private Shared Sub FindMsgBox(ByVal tid As Integer)
        ''--- Enumerate the windows owned by the UI thread
        EnumThreadWindows(tid, AddressOf EnumWindow, IntPtr.Zero)
    End Sub

    Private Shared Function EnumWindow(ByVal hWnd As IntPtr, ByVal lp As IntPtr) As Boolean
        ''--- Is this the message box?
        Dim sb As New StringBuilder(256)
        GetClassName(hWnd, sb, sb.Capacity)
        If sb.ToString() <> "#32770" Then Return True
        ''--- Got it, now find the buttons
        mLabelIndex = 0
        EnumChildWindows(hWnd, AddressOf FindButtons, IntPtr.Zero)
        Return False
    End Function

    Private Shared Function FindButtons(ByVal hWnd As IntPtr, ByVal lp As IntPtr) As Boolean
        Dim sb As New StringBuilder(256)
        GetClassName(hWnd, sb, sb.Capacity)
        If sb.ToString() = "Button" And mLabelIndex <= UBound(mLabels) Then
            ''--- Got one, update text
            SetWindowText(hWnd, mLabels(mLabelIndex))
            mLabelIndex += 1
        End If
        Return True
    End Function

    ''--- P/Invoke declarations
    Private Delegate Sub FindWindowDelegate(ByVal tid As Integer)
    Private Delegate Function EnumWindowDelegate(ByVal hWnd As IntPtr, ByVal lp As IntPtr) As Boolean
    Private Declare Auto Function EnumThreadWindows Lib "user32.dll" (ByVal tid As Integer, ByVal callback As EnumWindowDelegate, ByVal lp As IntPtr) As Boolean
    Private Declare Auto Function EnumChildWindows Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal callback As EnumWindowDelegate, ByVal lp As IntPtr) As Boolean
    Private Declare Auto Function GetClassName Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal name As StringBuilder, ByVal maxlen As Integer) As Integer
    Private Declare Auto Function GetCurrentThreadId Lib "kernel32.dll" () As Integer
    Private Declare Auto Function SetWindowText Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal text As String) As Boolean


End Class
