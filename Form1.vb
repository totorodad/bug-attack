REM Bug Attack
REM By: Jim Nolan, Copyright 2012
REM Contact: totorodad@gmail.com
REM www.sourceforge.net/

Public Class Form1

    Public ship_image_x As Integer = 727
    Public ship_image_y As Integer = 225
    Public ship_image_width As Integer = 42
    Public ship_image_height As Integer = 21
    Public ship_position As Integer = 274
    Public ship_y As Integer = 408

    Public bad1_image_x As Integer = 730
    Public bad1_image_y As Integer = 60
    Public bad1_image_width As Integer = 20
    Public bad1_image_height As Integer = 18

    Public bad2_image_x As Integer = 729
    Public bad2_image_y As Integer = 114
    Public bad2_image_width As Integer = 28
    Public bad2_image_height As Integer = 18

    Public bad3_image_x As Integer = 731
    Public bad3_image_y As Integer = 173
    Public bad3_image_width As Integer = 30
    Public bad3_image_height As Integer = 18

    Public bullet_image_x As Integer = 720
    Public bullet_image_y As Integer = 354
    Public bullet_image_width As Integer = 4
    Public bullet_image_height As Integer = 14

    Public bad_position As Integer = 0
    Public bad_max_right = 70
    Public bad_max_left = -100
    Public bads_moving_right As Boolean = True
    Public bads_moving_left As Boolean = False
    Public bad_life(11, 5) As Boolean
    Public bad_count As Integer = 55
    Public bomb_dropping As Boolean = False
    Public bomb_drop_timer2_count As Integer = 0
    Public bomb_position As Integer = 0
    Public bomb_altitude As Integer = 0
    Public Bomb_Interval As Integer = 1000

    Dim visible_bad_bm As New Bitmap(640, 480)
    Dim collision_detection_bad_bm As New Bitmap(640, 480)

    Public game_images(9) As Image

    Public barrier_image_x As Integer = 721
    Public barrier_image_y As Integer = 276
    Public barrier_image_width As Integer = 63
    Public barrier_image_height As Integer = 45

    Public barrier1_left As Integer = 78
    Public barrier1_right As Integer = barrier1_left + barrier_image_width
    Public barrier2_left As Integer = 78 + 1 * (63 + 75)
    Public barrier2_right As Integer = barrier2_left + barrier_image_width
    Public barrier3_left As Integer = 78 + 2 * (63 + 75)
    Public barrier3_right As Integer = barrier3_left + barrier_image_width
    Public barrier4_left As Integer = 78 + 3 * (63 + 75)
    Public barrier4_right As Integer = barrier4_left + barrier_image_width
    Public barrier_y As Integer = 307

    Public forward As Boolean = True

    Public score As Integer = 0
    Public lives As Integer = 3
    Public level As Integer = 1

    Public ship_right As Boolean = False
    Public ship_left As Boolean = False
    Public ship_fire As Boolean = False

    Public fire_position As Integer = 0
    Public ship_firing As Boolean = False

    Public missile_position As Integer

    Public game_over As Boolean = False

    REM Initialize the bitmaps for the game
    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim x As Integer
        Dim y As Integer

        load_ship()
        load_bad1()
        load_bad2()
        load_bad3()
        load_barrier()
        load_bullet()

        REM renew life off all bads
        For x = 0 To 10
            For y = 0 To 4
                bad_life(x, y) = True
            Next
        Next

        REM Create bad bitmaps
        create_visible_bad_bm()
        create_detection_bad_bm()

    End Sub

    REM Timer 1 is used to detect the user holding down the keys to move the ship
    Private Sub Timer1_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        If ship_right = True And ship_position <= 639 - 47 - 5 Then
            ship_position = ship_position + 5
        End If
        If ship_left = True And ship_position > 10 Then
            ship_position = ship_position - 5
        End If
        PictureBox1.Invalidate()
    End Sub

    REM This the main graphics paint event for the game
    Private Sub PictureBox1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles PictureBox1.Paint

        Dim i As Integer
        Dim font As Font
        font = New System.Drawing.Font("Terminal", 18, FontStyle.Bold)

        If (game_over = False) Then

            REM Draw Bad Bitmaps
            e.Graphics.DrawImageUnscaled(visible_bad_bm, bad_position, 0)

            REM Draw the ship
            e.Graphics.DrawImageUnscaled(game_images(0), ship_position, ship_y)

            REM draw barriers
            e.Graphics.DrawImageUnscaled(game_images(4), barrier1_left, barrier_y)
            e.Graphics.DrawImageUnscaled(game_images(5), barrier2_left, barrier_y)
            e.Graphics.DrawImageUnscaled(game_images(6), barrier3_left, barrier_y)
            e.Graphics.DrawImageUnscaled(game_images(7), barrier4_left, barrier_y)

            REM draw score
            e.Graphics.DrawString("SCORE: " + score.ToString, font, Brushes.White, 24, 8)

            REM draw level
            e.Graphics.DrawString("LEVEL: " + level.ToString, font, Brushes.White, 24, 35)

            REM draw lifes
            e.Graphics.DrawString("LIVES", font, Brushes.White, 338, 19)
            For i = 0 To lives - 1
                e.Graphics.DrawImageUnscaled(game_images(0), 427 + i * (42 + 13), 22)
            Next

            REM draw the lower line
            e.Graphics.DrawLine(Pens.Cyan, 0, 479 - 25, 639, 479 - 25)
            e.Graphics.DrawLine(Pens.Cyan, 0, 479 - 24, 639, 479 - 24)
            e.Graphics.DrawLine(Pens.Cyan, 0, 479 - 23, 639, 479 - 23)

            REM draw the missile if it is launced
            If ship_firing = True Then
                e.Graphics.DrawImageUnscaled(game_images(8), fire_position, missile_position)
            End If

            REM draw the bomb if it is dropping
            If bomb_dropping = True Then
                e.Graphics.DrawImageUnscaled(game_images(8), bomb_position, bomb_altitude)
            End If
        Else
            REM draw score
            e.Graphics.DrawString("GAME OVER", font, Brushes.White, 220, 100 + 0 * 50)
            e.Graphics.DrawString("YOUR SCORE: " + score.ToString, font, Brushes.White, 220, 100 + 1 * 50)
            e.Graphics.DrawString("YOUR LEVEL: " + level.ToString, font, Brushes.White, 220, 100 + 2 * 50)
            e.Graphics.DrawString("PRESS 'ENTER' TO PLAY AGAN", font, Brushes.White, 120, 100 + 3 * 50)
        End If
    End Sub

    REM create the bitmap for the bads
    Private Sub create_visible_bad_bm()
        Dim g As Graphics = Graphics.FromImage(visible_bad_bm)

        g.Clear(Color.Black)

        For i = 0 To 10
            If (bad_life(i, 0) = True) Then
                g.DrawImageUnscaled(game_images(1), 132 + i * (8 + 30), 74)
            End If

            If (bad_life(i, 1) = True) Then
                g.DrawImageUnscaled(game_images(1), 132 + i * (8 + 30), 74 + (1 * (17 + 18)))
            End If

            If (bad_life(i, 2) = True) Then
                g.DrawImageUnscaled(game_images(2), 127 + i * (8 + 30), 74 + (2 * (17 + 18)))
            End If

            If (bad_life(i, 3) = True) Then
                g.DrawImageUnscaled(game_images(3), 127 + i * (8 + 30), 74 + (3 * (17 + 18)))
            End If

            If (bad_life(i, 4) = True) Then
                g.DrawImageUnscaled(game_images(3), 127 + i * (8 + 30), 74 + (4 * (17 + 18)))
            End If
        Next
    End Sub

    REM create bitmap which uses the ARGB color to encode which bad is which for pixel vx. pixel collision detection
    Private Sub create_detection_bad_bm()
        Dim g As Graphics = Graphics.FromImage(collision_detection_bad_bm)

        g.Clear(Color.Black)

        For i = 0 To 10
            If (bad_life(i, 0) = True) Then
                pixelblitbox(Color.FromArgb(42, 42, i, 0), 132 + i * (8 + 30), 74, game_images(1).Width, game_images(1).Height)
            End If

            If (bad_life(i, 1) = True) Then
                pixelblitbox(Color.FromArgb(42, 42, i, 1), 127 + i * (8 + 30), 74 + (1 * (17 + 18)), game_images(2).Width, game_images(2).Height)
            End If

            If (bad_life(i, 2) = True) Then
                pixelblitbox(Color.FromArgb(42, 42, i, 2), 127 + i * (8 + 30), 74 + (2 * (17 + 18)), game_images(2).Width, game_images(3).Height)
            End If

            If (bad_life(i, 3) = True) Then
                pixelblitbox(Color.FromArgb(42, 42, i, 3), 127 + i * (8 + 30), 74 + (3 * (17 + 18)), game_images(3).Width, game_images(3).Height)
            End If

            If (bad_life(i, 4) = True) Then
                pixelblitbox(Color.FromArgb(42, 42, i, 4), 127 + i * (8 + 30), 74 + (4 * (17 + 18)), game_images(3).Width, game_images(3).Height)
            End If
        Next
    End Sub

    REM paint a color specific block on the collision dections bitmap (fillrectangle does not put a solid color)
    Private Sub pixelblitbox(ByVal p_color As Color, ByVal x As Integer, ByVal y As Integer, ByVal width As Integer, ByVal height As Integer)
        Dim xx As Integer
        Dim yy As Integer

        For xx = x To x + width - 1
            For yy = y To y + height - 1
                collision_detection_bad_bm.SetPixel(xx, yy, p_color)
            Next
        Next
    End Sub

    REM load the bitmap of the ship into the local bitmap
    Private Sub load_ship()
        REM load the images off from the game map in PictureBox2 into the game_images
        Dim x As Integer
        Dim y As Integer
        Dim p_color As Color

        Dim ship_bm As New Bitmap(ship_image_width + 1, ship_image_height + 1)
        Dim src_bm As Bitmap = PictureBox2.Image

        For x = ship_image_x To ship_image_x + ship_image_width - 1
            For y = ship_image_y To ship_image_y + ship_image_height - 1
                p_color = src_bm.GetPixel(x, y)
                ship_bm.SetPixel(x - ship_image_x, y - ship_image_y, p_color)
            Next
        Next
        game_images(0) = ship_bm
    End Sub

    REM load the bitmap of bad1 into the local bitmap
    Private Sub load_bad1()
        REM load the images off from the game map in PictureBox2 into the game_images
        Dim x As Integer
        Dim y As Integer
        Dim p_color As Color

        Dim bad1_bm As New Bitmap(bad1_image_width + 1, bad1_image_height + 1)
        Dim src_bm As Bitmap = PictureBox2.Image

        For x = bad1_image_x To bad1_image_x + bad1_image_width - 1
            For y = bad1_image_y To bad1_image_y + bad1_image_height - 1
                p_color = src_bm.GetPixel(x, y)
                bad1_bm.SetPixel(x - bad1_image_x, y - bad1_image_y, p_color)
            Next
        Next
        game_images(1) = bad1_bm
    End Sub

    REM load the bitmap of bad2 into the local bitmap
    Private Sub load_bad2()
        REM load the images off from the game map in PictureBox2 into the game_images
        Dim x As Integer
        Dim y As Integer
        Dim p_color As Color

        Dim bad2_bm As New Bitmap(bad2_image_width + 1, bad2_image_height + 1)
        Dim src_bm As Bitmap = PictureBox2.Image

        For x = bad2_image_x To bad2_image_x + bad2_image_width - 1
            For y = bad2_image_y To bad2_image_y + bad2_image_height - 1
                p_color = src_bm.GetPixel(x, y)
                bad2_bm.SetPixel(x - bad2_image_x, y - bad2_image_y, p_color)
            Next
        Next
        game_images(2) = bad2_bm
    End Sub

    REM load the bitmap of bad1 into the local bitmap
    Private Sub load_bad3()
        REM load the images off from the game map in PictureBox2 into the game_images
        Dim x As Integer
        Dim y As Integer
        Dim p_color As Color

        Dim bad3_bm As New Bitmap(bad3_image_width + 1, bad3_image_height + 1)
        Dim src_bm As Bitmap = PictureBox2.Image

        For x = bad3_image_x To bad3_image_x + bad3_image_width - 1
            For y = bad3_image_y To bad3_image_y + bad3_image_height - 1
                p_color = src_bm.GetPixel(x, y)
                bad3_bm.SetPixel(x - bad3_image_x, y - bad3_image_y, p_color)
            Next
        Next
        game_images(3) = bad3_bm
    End Sub

    REM load the bitmap of the barriers into the local bitmap
    Private Sub load_barrier()
        REM load the images off from the game map in PictureBox2 into the game_images
        Dim x As Integer
        Dim y As Integer
        Dim p_color As Color

        Dim barrier_bm As New Bitmap(barrier_image_width + 1, barrier_image_height + 1)
        Dim src_bm As Bitmap = PictureBox2.Image

        For x = barrier_image_x To barrier_image_x + barrier_image_width - 1
            For y = barrier_image_y To barrier_image_y + barrier_image_height - 1
                p_color = src_bm.GetPixel(x, y)
                barrier_bm.SetPixel(x - barrier_image_x, y - barrier_image_y, p_color)
            Next
        Next
        game_images(4) = bmpCopy(barrier_bm)
        game_images(5) = bmpCopy(barrier_bm)
        game_images(6) = bmpCopy(barrier_bm)
        game_images(7) = bmpCopy(barrier_bm)

    End Sub

    REM load the bitmap of the ships bullet (also bomb image) into the local bitmap
    Private Sub load_bullet()
        REM load the images off from the game map in PictureBox2 into the game_images
        Dim x As Integer
        Dim y As Integer
        Dim p_color As Color

        Dim bullet_bm As New Bitmap(bullet_image_width + 1, bullet_image_height + 1)
        Dim src_bm As Bitmap = PictureBox2.Image

        For x = bullet_image_x To bullet_image_x + bullet_image_width - 1
            For y = bullet_image_y To bullet_image_y + bullet_image_height - 1
                p_color = src_bm.GetPixel(x, y)
                bullet_bm.SetPixel(x - bullet_image_x, y - bullet_image_y, p_color)
            Next
        Next
        game_images(8) = bullet_bm
    End Sub

    REM a bitmap copy routine which is used to duplicate the barriers
    Public Function bmpCopy(ByVal srcBitmap As Bitmap) As Bitmap

        ' Create the new bitmap and associated graphics object
        Dim bmp As New Bitmap(srcBitmap.Width, srcBitmap.Height)
        Dim g As Graphics = Graphics.FromImage(bmp)

        ' Draw the specified section of the source bitmap to the new one
        g.DrawImageUnscaled(srcBitmap, 0, 0)

        ' Clean up
        g.Dispose()

        ' Return the bitmap
        Return bmp

    End Function

    REM detect which key is pressed on the main FORM1
    Private Sub Form1_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Left And game_over = False Then
            ship_left = True
            ship_right = False
        End If

        If e.KeyCode = Keys.Right And game_over = False Then
            ship_left = False
            ship_right = True
        End If

        REM detect a new fire only if the last firing isn't already being processed
        If e.KeyCode = Keys.Space And game_over = False Then
            If ship_firing = False Then
                ship_fire = True
            End If
        End If

        REM detect a ESC key to exit
        If e.KeyCode = Keys.Escape Then
            End
        End If

        REM if game over and hit return then restart game
        If e.KeyCode = Keys.Return And game_over = True Then
            reset_bads()
            level = 1
            lives = 3
            load_barrier()
            Timer1.Enabled = True
            bomb_drop_timer2_count = 0
            game_over = False
        End If
    End Sub

    REM detect when no key is pressed on the main FORM1
    Private Sub Form1_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyUp
        ship_left = False
        ship_right = False
        ship_fire = False
    End Sub

    REM Timer2 performes:
    REM 1) Loads and fires the bullet from the ship
    REM 2) Animates the movement of the bullet
    REM 3) If there is a bullet barrier collision then destroy part of the barrier
    REM 4) If there is a bullet bad collision then remove that bad.
    REM 5) Animate the bads back and forth
    REM 6) Retaliate: If 500ms has expired (50 Timer2 ticks) then drop a bomb on the ship from the nearest bad above the ship
    Private Sub Timer2_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Timer2.Tick

        REM Check Ship firing semaphore and if true load the inital settings for the bullet
        If ship_fire = True Then
            fire_position = ship_position + 19
            ship_fire = False
            ship_firing = True
            missile_position = 408
        End If

        REM animate ship firing
        If ship_firing = True Then
            If (missile_position >= 0) Then
                missile_position = missile_position - 10

                REM Check for barrier collision unless the missile is already past them
                If missile_position >= barrier_y And missile_position <= (barrier_y + barrier_image_height) Then
                    If barrier_collision() = True Then
                        ship_firing = False
                    End If
                    PictureBox1.Invalidate()
                End If

                REM Check for bad collision in the bad range
                If ship_firing = True And missile_position <= barrier_y And missile_position >= 74 Then
                    If bad_collision() = True Then
                        ship_firing = False
                    End If
                    PictureBox1.Invalidate()
                End If
            Else
                ship_firing = False
                PictureBox1.Invalidate()
            End If
        End If

        REM move the bad's
        If bads_moving_right = True Then
            If (bad_position <= bad_max_right) Then
                bad_position = bad_position + 1
                PictureBox1.Invalidate()
            Else
                bads_moving_right = False
                bads_moving_left = True
            End If
        End If

        If bads_moving_left = True Then
            If (bad_position >= bad_max_left) Then
                bad_position = bad_position - 1
                PictureBox1.Invalidate()
            Else
                bads_moving_right = True
                bads_moving_left = False
            End If
        End If

        REM drop bomb from bads if BOMB_INTERVAL [ms] have elapsed
        bomb_drop_timer2_count = bomb_drop_timer2_count + 10
        If bomb_dropping = False And bomb_drop_timer2_count >= Bomb_Interval Then
            choose_bomb_interval()
            bomb_dropping = True

            REM setup next bomb drop position to be in the center of the ship's current position
            bomb_position = ship_position + (ship_image_width / 2)
            bomb_altitude = starting_altitude()
            bomb_drop_timer2_count = 0
        End If

        REM if bomb_dropping is true then animate the 
        REM animate ship firing
        If bomb_dropping = True Then
            If (bomb_altitude < 460) Then
                bomb_altitude = bomb_altitude + 10

                REM Check for barrier collision unless the bomb is already past them
                If bomb_altitude >= barrier_y And bomb_altitude <= barrier_y + barrier_image_height Then
                    If bomb_barrier_collision() = True Then
                        bomb_dropping = False
                    End If
                    PictureBox1.Invalidate()
                End If

                REM Check for ship collision in the bad range
                If bomb_dropping = True And bomb_altitude >= ship_y And bomb_altitude <= ship_y + ship_image_height And bomb_position >= ship_position And bomb_position <= ship_position + ship_image_width Then
                    REM bomb barrier impact
                    lives = lives - 1
                    If lives = 0 Then
                        Timer1.Enabled = False
                        game_over = True
                    End If
                    bomb_dropping = False
                End If
                PictureBox1.Invalidate()
            Else
                bomb_dropping = False
                PictureBox1.Invalidate()
            End If
        End If
    End Sub

    REM Choose at random the time [ms] until the next bomb drops from the bads
    Private Sub choose_bomb_interval()
        Bomb_Interval = 200 + GetRandom(300, 700)
    End Sub

    REM give a random number between min and max
    Public Function GetRandom(ByVal Min As Integer, ByVal Max As Integer) As Integer
        Dim Generator As System.Random = New System.Random()
        Return Generator.Next(Min, Max)
    End Function

    REM determine the starting altitude for the bombs being dropped based on the nearest bad above the ship
    Private Function starting_altitude() As Integer
        Return 10
    End Function

    REM Detect ship bullet and bad collision
    Private Function bad_collision() As Boolean
        Dim x As Integer
        Dim y As Integer

        Dim p_color As Color = collision_detection_bad_bm.GetPixel(fire_position - bad_position, missile_position)

        If p_color.A = 42 And p_color.R = 42 Then
            x = CInt(p_color.G)
            y = CInt(p_color.B)
            If bad_life(x, y) = True Then
                bad_life(x, y) = False
                score = score + 100
                create_visible_bad_bm()
                create_detection_bad_bm()
                bad_count = bad_count - 1
                If bad_count = 0 Then
                    reset_bads()
                End If
            End If
            Return True
        End If

        Return False
    End Function

    REM If all bads have been eliminated then reload the bads and up the level
    Public Sub reset_bads()
        Dim x As Integer
        Dim y As Integer

        For x = 0 To 11
            For y = 0 To 4
                bad_life(x, y) = True
            Next
        Next
        create_visible_bad_bm()
        create_detection_bad_bm()
        bad_count = 55
        level = level + 1
    End Sub

    REM detect if there is a bomb barrier collision
    Private Function bomb_barrier_collision() As Boolean
        REM detect if there is a barrier collision at: bomb_position and missile_position
        Dim x_offset As Integer
        Dim y_offset As Integer = 10
        Dim bm As Bitmap
        Dim p_color As Color
        Dim x As Integer
        Dim y As Integer
        Dim blast_width As Integer = 5

        REM Check barrier1
        If (bomb_position >= barrier1_left And bomb_position <= barrier1_right) Then
            x_offset = bomb_position - barrier1_left
            bm = game_images(4)

            p_color = bm.GetPixel(x_offset, y_offset)
            If p_color = Color.FromArgb(255, 0, 255, 0) Then
                For y = 0 To barrier_image_height - 1
                    For x = x_offset - blast_width To x_offset + blast_width
                        If (x < bm.Width And x >= 0) Then
                            bm.SetPixel(x, y, Color.Black)
                        End If
                    Next
                Next
                Return True
            End If
        End If

        REM Check barrier2
        If (bomb_position >= barrier2_left And bomb_position <= barrier2_right) Then
            x_offset = bomb_position - barrier2_left
            bm = game_images(5)

            p_color = bm.GetPixel(x_offset, y_offset)
            If p_color = Color.FromArgb(255, 0, 255, 0) Then
                For y = 0 To barrier_image_height - 1
                    For x = x_offset - blast_width To x_offset + blast_width
                        If (x < bm.Width And x >= 0) Then
                            bm.SetPixel(x, y, Color.Black)
                        End If
                    Next
                Next
                Return True
            End If
        End If

        REM Check barrier3
        If (bomb_position >= barrier3_left And bomb_position <= barrier3_right) Then
            x_offset = bomb_position - barrier3_left
            bm = game_images(6)

            p_color = bm.GetPixel(x_offset, y_offset)
            If p_color = Color.FromArgb(255, 0, 255, 0) Then
                For y = 0 To barrier_image_height - 1
                    For x = x_offset - blast_width To x_offset + blast_width
                        If (x < bm.Width And x >= 0) Then
                            bm.SetPixel(x, y, Color.Black)
                        End If
                    Next
                Next
                Return True
            End If
        End If

        REM Check barrier4
        If (bomb_position >= barrier4_left And bomb_position <= barrier4_right) Then
            x_offset = bomb_position - barrier4_left
            bm = game_images(7)

            p_color = bm.GetPixel(x_offset, y_offset)
            If p_color = Color.FromArgb(255, 0, 255, 0) Then
                For y = 0 To barrier_image_height - 1
                    For x = x_offset - blast_width To x_offset + blast_width
                        If (x < bm.Width And x >= 0) Then
                            bm.SetPixel(x, y, Color.Black)
                        End If
                    Next
                Next
                Return True
            End If
        End If

        Return False
    End Function

    REM detect if there is a bullet barrier collision
    Private Function barrier_collision() As Boolean
        REM detect if there is a barrier collision at: fire_position and missile_position
        Dim x_offset As Integer
        Dim y_offset As Integer = (barrier_y + barrier_image_height) - missile_position
        Dim bm As Bitmap
        Dim p_color As Color
        Dim x As Integer
        Dim y As Integer
        Dim blast_width As Integer = 5

        REM Check barrier1
        If (fire_position >= barrier1_left And fire_position <= barrier1_right) Then
            x_offset = fire_position - barrier1_left
            bm = game_images(4)

            p_color = bm.GetPixel(x_offset, y_offset)
            If p_color = Color.FromArgb(255, 0, 255, 0) Then
                For y = 0 To barrier_image_height - 1
                    For x = x_offset - blast_width To x_offset + blast_width
                        If (x < bm.Width And x >= 0) Then
                            bm.SetPixel(x, y, Color.Black)
                        End If
                    Next
                Next
                Return True
            End If
        End If

        REM Check barrier2
        If (fire_position >= barrier2_left And fire_position <= barrier2_right) Then
            x_offset = fire_position - barrier2_left
            bm = game_images(5)

            p_color = bm.GetPixel(x_offset, y_offset)
            If p_color = Color.FromArgb(255, 0, 255, 0) Then
                For y = 0 To barrier_image_height - 1
                    For x = x_offset - blast_width To x_offset + blast_width
                        If (x < bm.Width And x >= 0) Then
                            bm.SetPixel(x, y, Color.Black)
                        End If
                    Next
                Next
                Return True
            End If
        End If

        REM Check barrier3
        If (fire_position >= barrier3_left And fire_position <= barrier3_right) Then
            x_offset = fire_position - barrier3_left
            bm = game_images(6)

            p_color = bm.GetPixel(x_offset, y_offset)
            If p_color = Color.FromArgb(255, 0, 255, 0) Then
                For y = 0 To barrier_image_height - 1
                    For x = x_offset - blast_width To x_offset + blast_width
                        If (x < bm.Width And x >= 0) Then
                            bm.SetPixel(x, y, Color.Black)
                        End If
                    Next
                Next
                Return True
            End If
        End If

        REM Check barrier4
        If (fire_position >= barrier4_left And fire_position <= barrier4_right) Then
            x_offset = fire_position - barrier4_left
            bm = game_images(7)

            p_color = bm.GetPixel(x_offset, y_offset)
            If p_color = Color.FromArgb(255, 0, 255, 0) Then
                For y = 0 To barrier_image_height - 1
                    For x = x_offset - blast_width To x_offset + blast_width
                        If (x < bm.Width And x >= 0) Then
                            bm.SetPixel(x, y, Color.Black)
                        End If
                    Next
                Next
                Return True
            End If
        End If

        Return False
    End Function

End Class
