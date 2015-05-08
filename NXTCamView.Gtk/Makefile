all: generated gschemas.compiled

gsettings.h: /usr/include/glib-2.0/gio/gsettings.h
	sed 's/GLIB_AVAILABLE_IN_\w\+//' < $< > $@ 

gsettings.raw: gsettings.sources gsettings.h
	gapi2-parser $<

gsettings.xml: gsettings.raw gsettings.metadata
	cp $< $@
	gapi2-fixup --api=$@ --metadata=$(word 2,$^)

generated: gsettings.xml
	rm -f $@/*
	gapi2-codegen --outdir=$@ `pkg-config --cflags gtk-sharp-2.0` --generate $<

glib-2.0/schemas:
	mkdir -p $@;

gschemas.compiled: glib-2.0/schemas nxtcamview.gschema.xml
	glib-compile-schemas --targetdir=$< .
