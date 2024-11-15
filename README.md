# Lern-Periode 7
## 25.10 bis 20.12


# Grob-Planung
Was die Noten angeht, ist es nicht sehr gut, aber das ist okay für mich, denn es ist ja erst der Anfang des Jahres. Von den Modulen her ist alles gut und ich mag alles, es gibt nichts, was besonders langweilig oder uninteressant ist. Für diese LP habe ich mir schon vor den Herbstferien etwas einfallen lassen. Ich möchte ein Flow Free Spiel auf der Basis von Unity2D neu erstellen.


## 25.10
Heute habe ich mich zum ersten Mal in meiner Zeit im Lern-Atelier direkt an die Arbeit gemacht, weil ich schon eine Idee hatte. Also habe ich ein Projekt in Unity2D erstellt, und zuerst habe ich beschlossen zu verstehen, wie genau das Originalspiel funktioniert. Sozusagen, um mir vorzustellen, was man sich normalerweise nicht vorstellt, wenn man ein Spiel spielt (ich meine konkret, wie jede Mechanik funktioniert und wie alles vom Standpunkt der Programmierung aus gesehen funktioniert). Danach habe ich beschlossen, die ersten Sprites zu zeichnen. Ich beschloss, dass das Spiel im PixelArt-Stil sein würde, also zeichnete ich ein Sprite für eine Zelle und ein Sprite für einen Punkt. Danach habe ich angefangen, den ersten Code zu schreiben, nämlich den GridManager.cs und das Grid.cs selbst, um das Gitter mit den Zellen bzw. die Zellen selbst zu erstellen.


## 01.11
- [x] Als Spieler möchte ich, dass das Spiel ein Raster erstellt, damit ich Platz zum Spielen habe. /
Erläuterung: Erstellen GridManager.cs, das für die Erstellung des Gitters verantwortlich ist.
- [x] Als Spieler möchte ich, dass das Gitter Zellen hat, in die man Punkte und Linien setzen kann. /
Erläuterung: Erstellen Cell.cs, das für jede einzelne Zelle zuständig ist.
- [x] Als Spieler möchte ich, dass die Zellen Punkte haben, damit ich mit ihnen interagieren kann. /
Erläuterung: Schreiben Code, um die Punkte zu erstellen.
- [ ] Als Spieler möchte ich in der Lage sein, Linien zwischen den Punkten zu ziehen, damit ich das Feld füllen und gewinnen kann. /
Erläuterung: Schreibe Code, um mit den Punkten zu interagieren.

Heute habe ich damit begonnen, die Sprites der Zellen zu ändern, damit sie normal aussehen. Dann habe ich Code geschrieben, um die Punkte zu erstellen und mit ihnen zu interagieren, aber ich bin auf das Problem gestoßen, dass die Punkte aus irgendeinem Grund nicht angezeigt werden. Es stellte sich heraus, dass das Problem darin bestand, dass der Shader nicht korrekt gerendert wurde und die Punkte nicht angezeigt wurden. Ich habe versucht, das Problem zu beheben, und das ist mir auch gelungen, aber die anderen Materialien und Shader sind für 3D optimiert und sehen in 2D absolut grauenhaft aus. Also beschloss ich, meinen eigenen Shader mit Hilfe von AI zu schreiben. Am Ende hat das bei mir auch nicht funktioniert. Um es kurz zu machen, die Punkte werden immer noch nicht angezeigt, und ich werde versuchen, dieses Problem nächste Woche zu lösen.


## 08.11
- [x] Als Spieler möchte ich, dass die Punkte auf dem Bildschirm angezeigt werden, damit ich sie sehen und später mit ihnen interagieren kann. /
Erläuterung: Lösen das Problem, dass die Punkte aus irgendeinem Grund nicht auf dem Bildschirm erscheinen.
- [x] Als Spieler möchte ich Linien ziehen können, um das Raster zu füllen und zu gewinnen. /
Erläuterung: Erstelle LineManager.cs und Line.cs, um Linien zu erstellen und zu verwalten.
- [x] Als Spieler möchte ich, dass die Linie die gleiche Farbe hat wie der Punkt, an dem ich mit dem Zeichnen der Linie beginne, damit die Linien mit den Punkten übereinstimmen. /
Erläuterung: Binden die Farbe jeder Linie an die Farbe jedes Punktpaares.
- [x] Als Spieler möchte ich, dass sich die Linien ausschliesslich auf dem Gitter bewegen können, damit ich zwei Punkte verbinden kann. /
Erläuterung: Ändere das Verhalten einer Linie, wenn ein Spieler sie von einem Startpunkt aus zieht (erstellt).

Der heutige Tag war sehr produktiv. Zunächst beschloss ich, mich mit dem Problem zu befassen, dass ich keine Punkte auf dem Bildschirm anzeigen kann, und verbrachte viel Zeit damit, bis ich mit einem Freund telefonierte, der mir den Tipp gab, ändere einfach die Transparenz. Ich hatte völlig vergessen, die Transparenz einzustellen, und habe so viel Zeit damit verbracht, das Problem mit buchstäblich zwei Klicks zu lösen. Aber ich liess mich nicht entmutigen und arbeitete weiter daran, dass der Spieler in der Lage war, Linien zu erstellen, die farblich an den Startpunkt angepasst waren, und dass sich die Linien genau auf dem Raster bewegten.


## 15.11
- [x] Als Spieler möchte ich, dass sich die Linien nicht kreuzen, damit ich das Raster füllen und gewinnen kann. /
Erläuterung: Erstelle eine Linienkreuzungsprüfung.
- [x] Als Spieler möchte ich, dass die Linien keine Felder durchqueren, auf denen sich eine Linie oder ein anderer Punkt als derjenige befindet, der mit meinem Startpunkt verbunden ist, damit ich die beiden Punkte verbinden kann. /
Erläuterung: Erstelle eine Prüfung auf Schnittpunkte von Punkten, die nicht benötigt werden.
- [x] Als Spieler möchte ich so viele Punkte auf dem Gitter haben, dass ich sie miteinander verbinden und das Feld füllen kann, um zu gewinnen. /
Erläuterung: Punktepaare bilden
- [x] Als Spieler möchte ich, dass sich die Punkte an Stellen befinden, an denen ich das Raster füllen kann, ohne die Linien zu überqueren, damit ich gewinnen kann. /
Erläuterung: Schaffe eine logische Anordnung von Punktepaaren.
- [x] Als Spieler möchte ich in der Lage sein zu gewinnen, und ich möchte auch benachrichtigt werden, wenn ich das Raster fülle, damit ich weiss, wann ich gewonnen habe. /
Erläuterung: Erstellen ein Canvas mit Text und einem entsprechenden Skript (GameCompletionChecker.cs)

Heute war auch ein recht produktiver Tag. Zunächst habe ich die Logik des Linienverhaltens auf dem Raster verfeinert (Prüfung auf Kreuzung mit einer anderen Linie und Prüfung auf Kreuzung mit einem unnötigen Punkt), dann habe ich genügend Punkte erstellt und sie so angeordnet, dass ich das Feld vollständig füllen konnte. Am Ende fügte ich eine Gewinnoption hinzu, und ich hatte die Kraft und Lust, dem Spieler eine Gewinnbenachrichtigung zu geben.


## 22.11
- [ ] Als Spieler möchte ich die Möglichkeit haben, die letzte Aktion rückgängig zu machen (die Zeile zu löschen), um den Fehler zu beheben und das Spiel fortzusetzen. /
Erläuterng: Schaffen die Möglichkeit, auf einen der verbundenen Punkte zu klicken, um alle Informationen über die zuvor erstellte Zeile zu löschen.
- [ ] Als Spieler möchte ich sehen können, wie viele Züge ich gemacht habe und welches Mindestergebnis für eine „perfekte“ Lösung erforderlich ist, um ein besseres Ergebnis anzustreben. /
Erläuterung: Erstellen ein System zum Zählen der Züge und zur Anzeige dieser Informationen auf dem Bildschirm
- [ ] Als Spieler möchte ich ein Menü haben, in dem ich die mir zur Verfügung stehenden Stufen auswählen kann, so dass ich die Macht habe, zu wählen. /
Erläuterung: Ein Menü erstellen; Mehrere Levels erstellen
- [ ] Letzte Woche habe ich ein extra Arbeitspaket gemacht.

✍️ Heute habe ich... (50-100 Wörter)


## 29.11
- [ ] 123
- [ ] 123
- [ ] 123
- [ ] 123

✍️ Heute habe ich... (50-100 Wörter)


## 06.12
- [ ] 123
- [ ] 123
- [ ] 123
- [ ] 123

✍️ Heute habe ich... (50-100 Wörter)


## 13.12
- [ ] 123
- [ ] 123
- [ ] 123
- [ ] 123

✍️ Heute habe ich... (50-100 Wörter)


## 20.12
- [ ] 123
- [ ] 123
- [ ] 123
- [ ] 123

✍️ Heute habe ich... (50-100 Wörter)


# Reflexion
