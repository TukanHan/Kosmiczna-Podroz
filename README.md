# Kosmiczna-Podroz
Program maj�cy na celu budowanie grafu i wyszukiwanie najkr�tszej drogi za pomoc� algorytmu Dijkstry w kosmicznej oprawie
graficznej

# O programie

### Cel Aplikacji

Aplikacja pozwala zbudowa� graf w do�� przyjaznej i lu�nej stylistyce kosmosu a tak�e znalezienie najkr�tszej drogi i zademonstrowanie jej w pocieszny spos�b przez przelot statku kosmicznego.

### Tworzenie grafu

U�ytkownik przeci�ga na plansze wierzcho�ki grafu kt�re s� reprezentowane przez planety i ��czy je kraw�dziami reprezentowanymi przez trasy.

Istniej� 2 rodzaje kraw�dzi:
* Skierowane - reprezentowane po��czeniem jednokierunkowym
* Nieskierowane - reprezentowane po��czeniem dwukierunkowym

W celu dokonania poprawek (usuni�cia) wierzcho�k�w lub kraw�dzi wystarczy przesun�� je poza ekran lub przej�� w tryb us�wania.

### Wyszukiwanie Drogi

Aby przej�� do trybu wyszukiwania drogi trzeba przeci�gn�� na map� ikon� statku i oznaczy� wierzcho�ek kt�ry b�dzie celem.

Program wyszukuje najkr�tsz� drog� za pomoc� algorytmu Dijkstry a jako wagi kraw�dzi s�u�� realne odleg�o�ci mi�dzy planetami.

Po dokonaniu oblicze� zostanie zaprezentowana najkr�tsza dost�pna trasa w formie podr�y statku do tego celu.

Z racji natury graf�w skierowanych, nie zawsze istnieje jakakolwiek droga, w tym przypadku statek pozostanie w miejscu.
