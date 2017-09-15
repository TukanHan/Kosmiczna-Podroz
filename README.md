# Kosmiczna-Podroz
Program maj¹cy na celu budowanie grafu i wyszukiwanie najkrótszej drogi za pomoc¹ algorytmu Dijkstry w kosmicznej oprawie
graficznej

# O programie

### Cel Aplikacji

Aplikacja pozwala zbudowaæ graf w doœæ przyjaznej i luŸnej stylistyce kosmosu a tak¿e znalezienie najkrótszej drogi i zademonstrowanie jej w pocieszny sposób przez przelot statku kosmicznego.

### Tworzenie grafu

U¿ytkownik przeci¹ga na plansze wierzcho³ki grafu które s¹ reprezentowane przez planety i ³¹czy je krawêdziami reprezentowanymi przez trasy.

Istniej¹ 2 rodzaje krawêdzi:
* Skierowane - reprezentowane po³¹czeniem jednokierunkowym
* Nieskierowane - reprezentowane po³¹czeniem dwukierunkowym

W celu dokonania poprawek (usuniêcia) wierzcho³ków lub krawêdzi wystarczy przesun¹æ je poza ekran lub przejœæ w tryb usówania.

### Wyszukiwanie Drogi

Aby przejœæ do trybu wyszukiwania drogi trzeba przeci¹gn¹æ na mapê ikonê statku i oznaczyæ wierzcho³ek który bêdzie celem.

Program wyszukuje najkrótsz¹ drogê za pomoc¹ algorytmu Dijkstry a jako wagi krawêdzi s³u¿¹ realne odleg³oœci miêdzy planetami.

Po dokonaniu obliczeñ zostanie zaprezentowana najkrótsza dostêpna trasa w formie podró¿y statku do tego celu.

Z racji natury grafów skierowanych, nie zawsze istnieje jakakolwiek droga, w tym przypadku statek pozostanie w miejscu.
